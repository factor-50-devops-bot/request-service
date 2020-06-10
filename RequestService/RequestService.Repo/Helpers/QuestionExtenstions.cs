﻿
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using RequestService.Repo.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Question = RequestService.Repo.EntityFramework.Entities.Question;
using SupportActivities = HelpMyStreet.Utils.Enums.SupportActivities;

namespace RequestService.Repo.Helpers
{
    public static class QuestionExtenstions
    {
        public static void SetQuestionData(this EntityTypeBuilder<Question> entity) {
            entity.HasData(new Question {
                Id = (int)Questions.SupportRequesting,
                Name = "Please tell us more about the help or support you're requesting",
                QuestionType = (int)QuestionType.MultiLineText,
                Required = false,
                AdditionalData = GetAdditionalData(Questions.SupportRequesting)
            });
            entity.HasData(new Question {
                Id = (int)Questions.FaceMask_SpecificRequirements,
                Name = "Please tell us about any specific requirements (e.g. colour, style etc.)",
                QuestionType = (int)QuestionType.MultiLineText,
                Required = false,
                AdditionalData = GetAdditionalData(Questions.FaceMask_SpecificRequirements)
            });
            entity.HasData(new Question {
                Id = (int)Questions.FaceMask_Amount,
                Name = "How many face coverings do you need?",
                QuestionType = (int)QuestionType.Number,
                Required = true,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Amount)
            });
            entity.HasData(new Question {
                Id = (int)Questions.FaceMask_Recipient,
                Name = "Who will be using the face coverings?",
                QuestionType = (int)QuestionType.Radio,
                Required = false,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Recipient)
            });
            entity.HasData(new Question { 
             Id = (int)Questions.FaceMask_Cost,
                Name = "Are you able to pay the cost of materials for your face covering (usually £2 - £3 each)?",
                QuestionType = (int)QuestionType.Radio,
                Required = false,
                AdditionalData = GetAdditionalData(Questions.FaceMask_Cost)
            });

            entity.HasData(new Question { 
            Id = (int)Questions.IsHealthCritical,
                Name = "Is this request critical to someone's health or wellbeing?",
                QuestionType = (int)QuestionType.Radio,
                Required = true,
                AdditionalData = GetAdditionalData(Questions.IsHealthCritical) 
            });
        }
        private static string GetAdditionalData(HelpMyStreet.Utils.Enums.Questions question)
        {
            List<AdditonalQuestionData> additionalData = new List<AdditonalQuestionData>();
            switch (question)
            {
                case Questions.FaceMask_Recipient:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "keyworkers",
                            Value = "Key workers"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "somonekeyworkers",
                            Value = "Someone helping key workers stay safe in their role (e.g. care home residents, visitors etc.)"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "someone",
                            Value = "Someone else"
                        },
                    };
                    break;
                case Questions.FaceMask_Cost:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "Yes",
                            Value = "Yes"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "No",
                            Value = "No"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "Contribution",
                            Value = "I can make a contribution"
                        },
                    };
                    break;
                case Questions.IsHealthCritical:
                    additionalData = new List<AdditonalQuestionData>
                    {
                        new AdditonalQuestionData
                        {
                            Key = "true",
                            Value = "Yes"
                        },
                        new AdditonalQuestionData
                        {
                            Key = "false",
                            Value = "No"
                        }
                    };
                    break;
            }

            return JsonConvert.SerializeObject(additionalData);
        }


        public static void SetActivityQuestionData(this EntityTypeBuilder<ActivityQuestions> entity)        
        {
            var activites = Enum.GetValues(typeof(SupportActivities)).Cast<SupportActivities>();

            foreach (var activity in activites)
            {
                if(activity == SupportActivities.FaceMask)
                {
                    entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.FaceMask_SpecificRequirements, Order = 1 });
                    entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.FaceMask_Amount, Order= 2 });                    
                    entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.FaceMask_Recipient, Order=3});
                    entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.FaceMask_Cost, Order= 4});
                    continue;

                }
                entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.SupportRequesting , Order= 1 });
                entity.HasData(new ActivityQuestions { ActivityId = (int)activity, QuestionId = (int)Questions.IsHealthCritical, Order = 2 });
            }
        }
    }
}
