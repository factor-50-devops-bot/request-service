﻿using HelpMyStreet.Contracts.ReportService.Response;
using HelpMyStreet.Contracts.RequestService.Request;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Extensions;
using HelpMyStreet.Utils.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RequestService.Core.Dto;
using RequestService.Core.Interfaces.Repositories;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SupportActivities = RequestService.Repo.EntityFramework.Entities.SupportActivities;

namespace RequestService.Repo
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetRequestPostCodeAsync(int requestId, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                return request.PostCode;
            }
            return null;
        }

        public async Task<int> CreateRequestAsync(string postCode, CancellationToken cancellationToken)
        {            
            Request request = new Request
            {
                PostCode = postCode,
                DateRequested = DateTime.Now,
                IsFulfillable = false,
                CommunicationSent = false,
            };

           _context.Request.Add(request);
            await _context.SaveChangesAsync(cancellationToken);
            return request.Id;
        }


        public async Task UpdateFulfillmentAsync(int requestId, Fulfillable fulfillable, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                request.FulfillableStatus = (byte)fulfillable;
                await _context.SaveChangesAsync(cancellationToken);
            }        
        }

        public async Task UpdateCommunicationSentAsync(int requestId, bool communicationSent, CancellationToken cancellationToken)
        {
            var request = await _context.Request.FirstAsync(x => x.Id == requestId, cancellationToken);
            if (request != null)
            {
                request.CommunicationSent = communicationSent;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdatePersonalDetailsAsync(PersonalDetailsDto dto, CancellationToken cancellationToken)
        {
            var personalDetails = new PersonalDetails
            {
                RequestId = dto.RequestID,
                FurtherDetails = dto.FurtherDetails,
                OnBehalfOfAnother = dto.OnBehalfOfAnother,
                RequestorEmailAddress = dto.RequestorEmailAddress,
                RequestorFirstName = dto.RequestorFirstName,
                RequestorLastName = dto.RequestorLastName,
                RequestorPhoneNumber = dto.RequestorPhoneNumber,
            };
            _context.PersonalDetails.Add(personalDetails);
            await _context.SaveChangesAsync(cancellationToken);               
        }

        public async Task AddSupportActivityAsync(SupportActivityDTO dto, CancellationToken cancellationToken)
        {
            List<SupportActivities> activties = new List<SupportActivities>();
            foreach(var activtity in dto.SupportActivities)
            {
                activties.Add(new SupportActivities
                {
                    RequestId = dto.RequestID,
                    ActivityId = (int)activtity
                });
            }

            _context.SupportActivities.AddRange(activties);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<ReportItem> GetDailyReport()
        {
            List<ReportItem> response = new List<ReportItem>();
            List<DailyReport> result = _context.DailyReport.ToList();

            if (result != null)
            {
                foreach (DailyReport dailyReport in result)
                {
                    response.Add(new ReportItem()
                    {
                        Section = dailyReport.Section,
                        Last2Hours = dailyReport.Last2Hours,
                        Today = dailyReport.Today,
                        SinceLaunch = dailyReport.SinceLaunch
                    });
                }
            }

            return response;
        }

        private Person GetPersonFromPersonalDetails(RequestPersonalDetails requestPersonalDetails)
        {
            return new Person()
            {
                FirstName = requestPersonalDetails.FirstName,
                LastName = requestPersonalDetails.LastName,
                EmailAddress = requestPersonalDetails.EmailAddress,
                AddressLine1 = requestPersonalDetails.Address.AddressLine1,
                AddressLine2 = requestPersonalDetails.Address.AddressLine2,
                AddressLine3 = requestPersonalDetails.Address.AddressLine3,
                Locality = requestPersonalDetails.Address.Locality,
                Postcode = requestPersonalDetails.Address.Postcode,
                MobilePhone = requestPersonalDetails.MobileNumber,
                OtherPhone = requestPersonalDetails.OtherNumber,
            };
        }

        public async Task<int> NewHelpRequestAsync(PostNewRequestForHelpRequest postNewRequestForHelpRequest, Fulfillable fulfillable)
        {
            Person requester = GetPersonFromPersonalDetails(postNewRequestForHelpRequest.HelpRequest.Requestor);
            Person recipient;
            
            if (postNewRequestForHelpRequest.HelpRequest.RequestorType == RequestorType.Myself)
            {
                recipient = requester;
            }
            else
            {
                recipient = GetPersonFromPersonalDetails(postNewRequestForHelpRequest.HelpRequest.Recipient);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Person.Add(requester);
                    _context.Person.Add(recipient);

                    Request request = new Request()
                    {
                        ReadPrivacyNotice = postNewRequestForHelpRequest.HelpRequest.ReadPrivacyNotice,
                        SpecialCommunicationNeeds = postNewRequestForHelpRequest.HelpRequest.SpecialCommunicationNeeds,
                        AcceptedTerms = postNewRequestForHelpRequest.HelpRequest.AcceptedTerms,
                        OtherDetails = postNewRequestForHelpRequest.HelpRequest.OtherDetails,
                        OrganisationName = postNewRequestForHelpRequest.HelpRequest.OrganisationName,
                        PostCode = postNewRequestForHelpRequest.HelpRequest.Recipient.Address.Postcode,
                        PersonIdRecipientNavigation = recipient,
                        PersonIdRequesterNavigation = requester,
                        RequestorType = (byte)postNewRequestForHelpRequest.HelpRequest.RequestorType,
                        FulfillableStatus = (byte)fulfillable,
                        CreatedByUserId = postNewRequestForHelpRequest.HelpRequest.CreatedByUserId,
                        ReferringGroupId = postNewRequestForHelpRequest.HelpRequest.ReferringGroupId,
                        Source = postNewRequestForHelpRequest.HelpRequest.Source
                    };

                    foreach (HelpMyStreet.Utils.Models.Job job in postNewRequestForHelpRequest.NewJobsRequest.Jobs)
                    {

                        EntityFramework.Entities.Job EFcoreJob = new EntityFramework.Entities.Job()
                        {
                            NewRequest = request,
                            Details = job.Details,
                            IsHealthCritical = job.HealthCritical,
                            SupportActivityId = (byte)job.SupportActivity,
                            DueDate = DateTime.Now.AddDays(job.DueDays),
                            JobStatusId = (byte) JobStatuses.Open
                        };
                        _context.Job.Add(EFcoreJob);
                        await _context.SaveChangesAsync();
                        job.JobID = EFcoreJob.Id;

                        foreach (var question in job.Questions)
                        {
                            _context.JobQuestions.Add(new JobQuestions
                            {
                                Job = EFcoreJob,
                                QuestionId = question.Id,
                                Answer = question.Answer
                            });
                        }

                        _context.RequestJobStatus.Add(new RequestJobStatus()
                        {
                            DateCreated = DateTime.Now,
                            JobStatusId = (byte)JobStatuses.Open,
                            Job = EFcoreJob,
                            CreatedByUserId = postNewRequestForHelpRequest.HelpRequest.CreatedByUserId,
                        });
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return request.Id;
                }
                catch(Exception exc)
                {
                    transaction.Rollback();
                }
            }
            throw new Exception("Unable to save request");
        }

        private void AddJobStatus(int jobID, int? createdByUserID, int? volunteerUserID, byte jobStatus)
        {
            _context.RequestJobStatus.Add(new RequestJobStatus()
            {
                CreatedByUserId = createdByUserID,
                VolunteerUserId = volunteerUserID,
                JobId = jobID,
                JobStatusId = jobStatus
            });
        }

        public async Task<List<ActivityQuestionDTO>> GetActivityQuestions(List<HelpMyStreet.Utils.Enums.SupportActivities> activity, RequestHelpFormVariant requestHelpFormVariant, RequestHelpFormStage requestHelpFormStage, CancellationToken cancellationToken)
        {
            return await _context.ActivityQuestions
                                    .Where(x => activity.Any(a => (int)a == x.ActivityId) && x.RequestFormVariantId == (int)requestHelpFormVariant && x.RequestFormStageId == (int)requestHelpFormStage)
                                    .GroupBy(x => x.ActivityId).Select(g => new ActivityQuestionDTO
            {
                Activity = (HelpMyStreet.Utils.Enums.SupportActivities)g.Key,
                Questions = g.OrderBy(x => x.Order).Select(x => new HelpMyStreet.Utils.Models.Question
                {
                    Id = x.Question.Id,
                    Name = x.Question.Name,
                    Required = x.Required,
                    SubText = x.Subtext,
                    Location = x.Location,
                    PlaceholderText = x.PlaceholderText,
                    Type = (QuestionType)x.Question.QuestionType,
                    AddtitonalData = x.Question.AdditionalData != null ? JsonConvert.DeserializeObject<List<AdditonalQuestionData>>(x.Question.AdditionalData) : new List<AdditonalQuestionData>()
                }).ToList()
            }).ToListAsync(cancellationToken);
            
        }

        public async Task<bool> UpdateJobStatusOpenAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            bool response = false;
            byte openJobStatus = (byte)JobStatuses.Open;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != openJobStatus)
                {
                    job.JobStatusId = openJobStatus;
                    job.VolunteerUserId = null;
                    AddJobStatus(jobID, createdByUserID, null, openJobStatus);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }

        public async Task<bool> UpdateJobStatusCancelledAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            bool response = false;
            byte openJobStatus = (byte)JobStatuses.Cancelled;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != openJobStatus)
                {
                    job.JobStatusId = openJobStatus;
                    job.VolunteerUserId = null;
                    AddJobStatus(jobID, createdByUserID, null, openJobStatus);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }

        public async Task<bool> UpdateJobStatusInProgressAsync(int jobID, int createdByUserID, int volunteerUserID, CancellationToken cancellationToken)
        {
            bool response = false;
            byte inProgressJobStatus = (byte)JobStatuses.InProgress;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != inProgressJobStatus)
                {
                    job.JobStatusId = inProgressJobStatus;
                    job.VolunteerUserId = volunteerUserID;
                    AddJobStatus(jobID, createdByUserID, volunteerUserID, inProgressJobStatus);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }

        public async Task<bool> UpdateJobStatusDoneAsync(int jobID, int createdByUserID, CancellationToken cancellationToken)
        {
            bool response = false;
            byte doneJobStatus = (byte)JobStatuses.Done;
            var job = _context.Job.Where(w => w.Id == jobID).FirstOrDefault();
            if (job != null)
            {
                if (job.JobStatusId != doneJobStatus)
                {
                    job.JobStatusId = doneJobStatus;
                    AddJobStatus(jobID, createdByUserID, null, doneJobStatus);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    if (result == 2)
                    {
                        response = true;
                    }
                }
            }
            return response;
        }

        public List<JobSummary> GetJobsAllocatedToUser(int volunteerUserID)
        {
            byte jobStatusID_InProgress = (byte)JobStatuses.InProgress;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.NewRequest)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.VolunteerUserId == volunteerUserID 
                                                && w.JobStatusId == jobStatusID_InProgress
                                            ).ToList();

            return GetJobSummaries(jobSummaries);
            
        }

        public List<JobSummary> GetOpenJobsSummaries()
        {
            
            byte jobStatusID_Open = (byte)JobStatuses.Open;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.JobAvailableToGroup)
                                    .Include(i => i.NewRequest)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.JobStatusId == jobStatusID_Open
                                            ).ToList();
            return GetJobSummaries(jobSummaries);
            
        }

        public List<JobSummary> GetJobsInProgressSummaries()
        {
            byte jobStatusID_InProgress = (byte)JobStatuses.InProgress;

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.JobAvailableToGroup)
                                    .Include(i => i.NewRequest)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w => w.JobStatusId == jobStatusID_InProgress
                                            ).ToList();
            return GetJobSummaries(jobSummaries);
        }

        public List<JobSummary> GetJobsByStatusesSummaries(List<JobStatuses> jobStatuses)
        {
            List<byte> statuses = new List<byte>();

            foreach(JobStatuses js in jobStatuses)
            {
                statuses.Add((byte)js);
            }

            List<EntityFramework.Entities.Job> jobSummaries = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .Include(i => i.JobAvailableToGroup)
                                    .Include(i => i.NewRequest)
                                    .Include(i => i.JobQuestions)
                                    .ThenInclude(rq => rq.Question)
                                    .Where(w =>statuses.Contains(w.JobStatusId.Value)
                                            ).ToList();
            return GetJobSummaries(jobSummaries);
        }

        private List<byte> ConvertSupportActivities(List<HelpMyStreet.Utils.Enums.SupportActivities> supportActivities)
        {
            List<byte> activities = new List<byte>();

            foreach (HelpMyStreet.Utils.Enums.SupportActivities sa in supportActivities)
            {
                activities.Add((byte)sa);
            }
            return activities;
        }

        private List<byte> ConvertJobStatuses(List<JobStatuses> jobStatuses)
        {
            List<byte> statuses = new List<byte>();

            foreach (JobStatuses sa in jobStatuses)
            {
                statuses.Add((byte)sa);
            }
            return statuses;
        }

        public List<JobHeader> GetJobHeaders(GetJobsByFilterRequest request)
        {
            var jobHeaders = _context.Job
                                    .Include(i => i.RequestJobStatus)
                                    .If(request.Groups!=null, q => q.Include(e => e.JobAvailableToGroup))                                     
                                    .Include(i => i.NewRequest);

            IQueryable<EntityFramework.Entities.Job> filtered = jobHeaders;

            if (request.UserID.HasValue)
            {
                filtered = jobHeaders.Where(w => w.VolunteerUserId == request.UserID.Value);
            }

            if (request.SupportActivities?.SupportActivities.Count > 0)
            {
                filtered = filtered.Where(w => ConvertSupportActivities(request.SupportActivities.SupportActivities).Contains(w.SupportActivityId));
            }

            if (request.ReferringGroupID.HasValue)
            {
                filtered = filtered.Where(w => w.NewRequest.ReferringGroupId == request.ReferringGroupID.Value);
            }

            if (request.Groups != null)
            {
                filtered = filtered.Where(t2 => request.Groups.Groups.Any(t1 => t2.JobAvailableToGroup.Select(x => x.GroupId).Contains(t1)));
            }

            if (request.JobStatuses != null)
            {
                filtered = filtered.Where(t2 => ConvertJobStatuses(request.JobStatuses.JobStatuses).Contains(t2.JobStatusId.Value));
            }

            return GetJobHeaders(filtered);
        }

        public List<JobHeader> GetJobHeaders(IQueryable<EntityFramework.Entities.Job> jobs)
        {
            List<JobHeader> response = new List<JobHeader>();
            foreach (EntityFramework.Entities.Job j in jobs)
            {
                response.Add(MapEFJobToJobHeader(j));
            }
            return response;
        }

        private JobHeader MapEFJobToJobHeader(EntityFramework.Entities.Job job)
        {
            return new JobHeader()
            {
                IsHealthCritical = job.IsHealthCritical,
                DueDate = job.DueDate,
                JobID = job.Id,
                JobStatus = (JobStatuses)job.JobStatusId,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,
                PostCode = job.NewRequest.PostCode,
                ReferringGroupID = job.NewRequest.ReferringGroupId,
                DateStatusLastChanged = job.RequestJobStatus.Max(x => x.DateCreated),
                DateRequested = job.NewRequest.DateRequested,
                Archive = job.NewRequest.Archive
            };
        }

        private JobSummary MapEFJobToSummary(EntityFramework.Entities.Job job)
        {
            return new JobSummary()
            {
                IsHealthCritical = job.IsHealthCritical,
                DueDate = job.DueDate,
                Details = job.Details,
                JobID = job.Id,
                VolunteerUserID = job.VolunteerUserId,
                JobStatus = (JobStatuses)job.JobStatusId,
                SupportActivity = (HelpMyStreet.Utils.Enums.SupportActivities)job.SupportActivityId,
                PostCode = job.NewRequest.PostCode,
                Questions = MapToQuestions(job.JobQuestions),
                ReferringGroupID = job.NewRequest.ReferringGroupId,
                Groups = job.JobAvailableToGroup.Select(x => x.GroupId).ToList(),
                RecipientOrganisation = job.NewRequest.OrganisationName,
                DateStatusLastChanged = job.RequestJobStatus.Max(x => x.DateCreated),
                DueDays = Convert.ToInt32((job.DueDate.Date - DateTime.Now.Date).TotalDays),
                DateRequested = job.NewRequest.DateRequested,
                RequestorType = (RequestorType)job.NewRequest.RequestorType,
                Archive = job.NewRequest.Archive
            };
        }

        public List<JobSummary> GetJobSummaries(List<EntityFramework.Entities.Job> jobs)
        {
            List<JobSummary> response = new List<JobSummary>();
            foreach (EntityFramework.Entities.Job j in jobs)
            {
                response.Add(MapEFJobToSummary(j));
            }
            return response;
        }

        private List<HelpMyStreet.Utils.Models.Question> MapToQuestions(ICollection<JobQuestions> questions)
        {
            return questions.Select(x => new HelpMyStreet.Utils.Models.Question
            {
                Id = x.QuestionId,
                Answer = x.Answer,
                Name = x.Question.Name,
                //Required = x.Question.Required,
                Type = (QuestionType)x.Question.QuestionType,
                AddtitonalData = JsonConvert.DeserializeObject<List<AdditonalQuestionData>>(x.Question.AdditionalData),
            }).ToList();
        }

        private RequestPersonalDetails GetPerson(Person person)
        {
            return new RequestPersonalDetails()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                EmailAddress = person.EmailAddress,
                MobileNumber = person.MobilePhone,
                OtherNumber = person.OtherPhone,
                Address = new Address()
                {
                    AddressLine1 = person.AddressLine1,
                    AddressLine2 = person.AddressLine2,
                    AddressLine3 = person.AddressLine3,
                    Locality = person.Locality,
                    Postcode = person.Postcode
                }
            };
        }

        public GetJobDetailsResponse GetJobDetails(int jobID)
        {
            GetJobDetailsResponse response = new GetJobDetailsResponse();
            var efJob = _context.Job
                        .Include(i=> i.RequestJobStatus)
                        .Include(i => i.JobQuestions)
                        .ThenInclude(rq => rq.Question)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i => i.PersonIdRecipientNavigation)
                        .Include(i => i.NewRequest)
                        .ThenInclude(i=> i.PersonIdRequesterNavigation)
                        .Where(w => w.Id == jobID).FirstOrDefault();

            if(efJob == null)
            {
                return response;
            }

            bool isArchived = false;
            if(efJob.NewRequest.Archive.HasValue)
            {
                isArchived = efJob.NewRequest.Archive.Value;
            }

            response = new GetJobDetailsResponse()
            {
                JobSummary = MapEFJobToSummary(efJob),
                Recipient = isArchived ? null:  GetPerson(efJob.NewRequest.PersonIdRecipientNavigation),
                Requestor = isArchived ? null : GetPerson(efJob.NewRequest.PersonIdRequesterNavigation),
                History = GetJobStatusHistory(efJob.RequestJobStatus.ToList())
            };

            return response;
        }

        public Task<List<LatitudeAndLongitudeDTO>> GetLatitudeAndLongitudes(List<string> postCodes, CancellationToken cancellationToken)
        {
            var postcodeDetails = (from a in _context.Postcode
                                   where postCodes.Any(p => p == a.Postcode)
                                   select new LatitudeAndLongitudeDTO
                                   {
                                       Postcode = a.Postcode,
                                       Latitude = Convert.ToDouble(a.Latitude),
                                       Longitude = Convert.ToDouble(a.Longitude)
                                   }).ToListAsync(cancellationToken);

            return postcodeDetails;
        }

        public async Task AddJobAvailableToGroupAsync(int jobID, int groupID, CancellationToken cancellationToken)
        {
            _context.JobAvailableToGroup.Add(new JobAvailableToGroup()
            {
                GroupId = groupID,
                JobId = jobID
            });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public List<StatusHistory> GetJobStatusHistory(int jobID)
        {
            return _context.RequestJobStatus.Where(x => x.JobId == jobID)
                .Select(x => new StatusHistory
                {
                    JobStatus = (JobStatuses) x.JobStatusId,
                    StatusDate = x.DateCreated,
                    VolunteerUserID = x.VolunteerUserId,
                    CreatedByUserID = x.CreatedByUserId
                }).ToList();
        }

        public List<StatusHistory> GetJobStatusHistory(List<RequestJobStatus> requestJobStatus)
        {
            return requestJobStatus
                .Select(x => new StatusHistory
                {
                    JobStatus = (JobStatuses)x.JobStatusId,
                    StatusDate = x.DateCreated,
                    VolunteerUserID = x.VolunteerUserId,
                    CreatedByUserID = x.CreatedByUserId
                }).ToList();
        }

        public async Task<List<int>> GetGroupsForJobAsync(int jobID, CancellationToken cancellationToken)
        {
            return _context.JobAvailableToGroup.Where(x => x.JobId == jobID)
                .Select(x => x.GroupId).ToList();
        }

        public async Task<int?> GetReferringGroupIDForJobAsync(int jobID, CancellationToken cancellationToken)
        {
            int? referringGroupId = null;
            var job = await _context.Job
                .Include(x => x.NewRequest)
                .FirstAsync(x => x.Id == jobID);

            if(job!=null)
            {
                referringGroupId = job.NewRequest.ReferringGroupId;
            }

            return referringGroupId;

        }

        public void ArchiveOldRequests(int daysSinceJobRequested, int daysSinceJobStatusChanged)
        {
            DateTime dtExpire = DateTime.Now.AddDays(-daysSinceJobRequested);
            var requests =_context.Request
                .Include(x=> x.Job)
                .ThenInclude(x=> x.RequestJobStatus)
                .Where(x => (x.Archive ?? false == false) 
                && ((x.PersonIdRecipient.HasValue || x.PersonIdRequester.HasValue)) 
                && x.DateRequested < dtExpire)
                .ToList();

            foreach(Request r in requests)
            {
                foreach(EntityFramework.Entities.Job j in r.Job)
                {
                    bool inactive = j.RequestJobStatus.Min(x => (DateTime.Now.Date - x.DateCreated.Date).TotalDays > daysSinceJobStatusChanged);

                    if(inactive && (r.Archive ?? false) == false)
                    {
                        r.Archive = true;
                        _context.Request.Update(r);
                    }
                }
            }
            _context.SaveChanges();
        }

        public GetJobSummaryResponse GetJobSummary(int jobID)
        {
            GetJobSummaryResponse response = new GetJobSummaryResponse();
            var efJob = _context.Job
                        .Include(i => i.RequestJobStatus)
                        .Include(i => i.JobQuestions)
                        .ThenInclude(rq => rq.Question)
                        .Include(i => i.NewRequest)
                        .Where(w => w.Id == jobID).FirstOrDefault();

            response = new GetJobSummaryResponse()
            {
                JobSummary = MapEFJobToSummary(efJob)
            };

            return response;
        }
    }
}
