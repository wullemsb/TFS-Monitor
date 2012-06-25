// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.DPE.ODataTFS.Model.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Server;

    public static class EntityTranslator
    {
        public static Project ToModel(this ProjectInfo projectInfo, string collectionName)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            return new Project()
            {
                Collection = collectionName,
                Name = projectInfo.Name,
            };
        }

        public static AreaPath ToModel(this XmlNode node, IEnumerable<AreaPath> subAreas)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            var pathElements = node.Attributes["Path"].Value.Trim('\\').Split('\\');
            if (pathElements != null && pathElements.Length > 1 && pathElements.ElementAt(1).Equals("Area"))
            {
                var parsedAreaPath = pathElements.ToList();
                parsedAreaPath.RemoveAt(1);
                pathElements = parsedAreaPath.ToArray();
            }

            return new AreaPath() { Name = node.Attributes["Name"].Value, Path = EncodePath(string.Join("\\", pathElements)), SubAreas = subAreas };
        }

        public static Build ToModel(this TeamFoundation.Build.Client.IBuildDetail buildDetail)
        {
            if (buildDetail == null)
            {
                throw new ArgumentNullException("buildDetail");
            }

            return new Build()
            {
                Project = buildDetail.TeamProject,
                Definition = buildDetail.BuildDefinition.Name,
                Number = buildDetail.BuildNumber,
                Reason = buildDetail.Reason.ToString(),
                Quality = buildDetail.Quality,
                Status = buildDetail.Status.ToString(),
                RequestedBy = buildDetail.RequestedBy,
                RequestedFor = buildDetail.RequestedFor,
                LastChangedBy = buildDetail.LastChangedBy,
                StartTime = buildDetail.StartTime,
                FinishTime = buildDetail.FinishTime,
                LastChangedOn = buildDetail.LastChangedOn,
                BuildFinished = buildDetail.BuildFinished,
                DropLocation = buildDetail.DropLocation,
                Errors = string.Join(Environment.NewLine, TeamFoundation.Build.Client.InformationNodeConverters.GetBuildErrors(buildDetail).Select(e => e.Message)),
                Warnings = string.Join(Environment.NewLine, TeamFoundation.Build.Client.InformationNodeConverters.GetBuildWarnings(buildDetail).Select(w => w.Message))
            };
        }

        public static BuildDefinition ToModel(this TeamFoundation.Build.Client.IBuildDefinition buildDefinition)
        {
            if (buildDefinition == null)
            {
                throw new ArgumentNullException("buildDefinition");
            }

            return new BuildDefinition()
            {
                Project = buildDefinition.TeamProject,
                Definition = buildDefinition.Name
            };
        }

        public static Change ToModel(this TeamFoundation.VersionControl.Client.Change tfsChange, string collectionName, int changesetId)
        {
            if (tfsChange == null)
            {
                throw new ArgumentNullException("tfsChange");
            }

            return new Change()
            {
                Collection = collectionName,
                Changeset = changesetId,
                Path = EncodePath(tfsChange.Item.ServerItem),
                Type = tfsChange.Item.ItemType.ToString(),
                ChangeType = tfsChange.ChangeType.ToString()
            };
        }

        public static Changeset ToModel(this TeamFoundation.VersionControl.Client.Changeset tfsChangeset, Uri changesetWebEditorUrl)
        {
            if (tfsChangeset == null)
            {
                throw new ArgumentNullException("tfsChangeset");
            }

            if (changesetWebEditorUrl == null)
            {
                throw new ArgumentNullException("changesetWebEditorUrl");
            }

            return new Changeset()
            {
                ArtifactUri = tfsChangeset.ArtifactUri.ToString(),
                Comment = tfsChangeset.Comment,
                Committer = tfsChangeset.Committer,
                CreationDate = tfsChangeset.CreationDate,
                Id = tfsChangeset.ChangesetId,
                Owner = tfsChangeset.Owner,
                WebEditorUrl = changesetWebEditorUrl.ToString()
            };
        }

        public static WorkItem ToModel(this TeamFoundation.WorkItemTracking.Client.WorkItem tfsWorkItem, Uri workItemWebEditorUrl)
        {
            if (tfsWorkItem == null)
            {
                throw new ArgumentNullException("tfsWorkItem");
            }

            if (workItemWebEditorUrl == null)
            {
                throw new ArgumentNullException("workItemWebEditorUrl");
            }

            return new WorkItem
            {
                Id = tfsWorkItem.Id,
                AreaPath = tfsWorkItem.AreaPath,
                IterationPath = tfsWorkItem.IterationPath,
                Revision = tfsWorkItem.Revision,
                Priority = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.Priority") ?? tfsWorkItem.Fields.GetFieldValueByReference("CodeStudio.Rank"),
                Severity = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.Severity"),
                StackRank = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.StackRank"),
                Project = tfsWorkItem.Project.Name,
                AssignedTo = tfsWorkItem.Fields[TeamFoundation.WorkItemTracking.Client.CoreField.AssignedTo].Value.ToString(),
                CreatedBy = tfsWorkItem.CreatedBy,
                CreatedDate = tfsWorkItem.CreatedDate,
                ChangedBy = tfsWorkItem.ChangedBy,
                ChangedDate = tfsWorkItem.ChangedDate,
                ResolvedBy = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.ResolvedBy"),
                Title = tfsWorkItem.Title,
                State = tfsWorkItem.State,
                Type = tfsWorkItem.Type.Name,
                Reason = tfsWorkItem.Reason,
                Description = tfsWorkItem.Description,
                ReproSteps = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.TCM.ReproSteps"),
                FoundInBuild = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Build.FoundIn"),
                IntegratedInBuild = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Build.IntegrationBuild"),
                WebEditorUrl = workItemWebEditorUrl.ToString()
            };
        }

        public static Attachment ToModel(this TeamFoundation.WorkItemTracking.Client.Attachment tfsAttachment, int workItemId, int index)
        {
            if (tfsAttachment == null)
            {
                throw new ArgumentNullException("tfsAttachment");
            }

            return new Attachment
            {
                Id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", workItemId, index),
                WorkItemId = workItemId,
                Index = index,
                Name = tfsAttachment.Name,
                Extension = tfsAttachment.Extension,
                Comment = tfsAttachment.Comment,
                Length = tfsAttachment.Length,
                AttachedTime = tfsAttachment.AttachedTime,
                CreationTime = tfsAttachment.CreationTime,
                LastWriteTime = tfsAttachment.LastWriteTime,
                Uri = tfsAttachment.Uri.ToString()
            };
        }

        public static Query ToModel(this TeamFoundation.WorkItemTracking.Client.QueryDefinition tfsQueryItem, string path)
        {
            if (tfsQueryItem == null)
            {
                throw new ArgumentNullException("tfsQueryItem");
            }

            return new Query
            {
                Id = tfsQueryItem.Id.ToString(),
                Name = tfsQueryItem.Name,
                QueryText = tfsQueryItem.QueryText,
                Project = tfsQueryItem.Project.Name,
                Path = path,
                QueryType = tfsQueryItem.QueryType.ToString()
            };
        }

        public static Branch ToModel(this TeamFoundation.VersionControl.Client.BranchObject tfsBranch)
        {
            if (tfsBranch == null)
            {
                throw new ArgumentNullException("tfsBranch");
            }

            return new Branch
            {
                Path = EncodePath(tfsBranch.Properties.RootItem.Item),
                Description = tfsBranch.Properties.Description,
                DateCreated = tfsBranch.DateCreated
            };
        }

        public static TeamFoundation.WorkItemTracking.Client.WorkItem ToEntity(this WorkItem workItemModel, Microsoft.TeamFoundation.WorkItemTracking.Client.Project project)
        {
            if (workItemModel == null)
            {
                throw new ArgumentNullException("workItemModel");
            }

            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            var workItemTypes = project.WorkItemTypes;
            var type = workItemTypes[workItemModel.Type];
            var workItemEntity = new TeamFoundation.WorkItemTracking.Client.WorkItem(type);
            
            workItemEntity.UpdateFromModel(workItemModel);
           
            return workItemEntity;
        }

        public static TeamFoundation.WorkItemTracking.Client.Attachment ToEntity(this Attachment attachmentModel, string path)
        {
            if (attachmentModel == null)
            {
                throw new ArgumentNullException("attachmentModel");
            }

            return new TeamFoundation.WorkItemTracking.Client.Attachment(path, attachmentModel.Comment);
        }

        public static void UpdateFromModel(this TeamFoundation.WorkItemTracking.Client.WorkItem workItemEntity, WorkItem workItemModel)
        {
            if (workItemEntity == null)
            {
                throw new ArgumentNullException("workItemEntity");
            }

            if (workItemModel == null)
            {
                throw new ArgumentNullException("workItemModel");
            }

            workItemEntity.Title = workItemModel.Title;
            workItemEntity.Description = workItemModel.Description;

            if (!string.IsNullOrWhiteSpace(workItemModel.AreaPath))
            {
                workItemEntity.AreaPath = workItemModel.AreaPath;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.IterationPath))
            {
                workItemEntity.IterationPath = workItemModel.IterationPath;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.State))
            {
                workItemEntity.State = workItemModel.State;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Reason))
            {
                workItemEntity.Reason = workItemModel.Reason;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.AssignedTo))
            {
                workItemEntity.Fields[TeamFoundation.WorkItemTracking.Client.CoreField.AssignedTo].Value = workItemModel.AssignedTo;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Priority))
            {
                int priority;
                if (int.TryParse(workItemModel.Priority, NumberStyles.Integer, CultureInfo.InvariantCulture, out priority))
                {
                    workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.Common.Priority", priority);

                    // For CodePlex TFS
                    workItemEntity.Fields.SetFieldValueByReference("CodeStudio.Rank", GetPriorityDescription(priority));
                }
                else
                {
                    // For CodePlex TFS
                    workItemEntity.Fields.SetFieldValueByReference("CodeStudio.Rank", workItemModel.Priority);
                }
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Severity))
            {
                workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.Common.Severity", workItemModel.Severity);
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.StackRank))
            {
                workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.Common.StackRank", double.Parse(workItemModel.StackRank, NumberStyles.Float, CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.ReproSteps))
            {
                workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.TCM.ReproSteps", workItemModel.ReproSteps);
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.FoundInBuild))
            {
                workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.Build.FoundIn", workItemModel.FoundInBuild);
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.IntegratedInBuild))
            {
                workItemEntity.Fields.SetFieldValueByReference("Microsoft.VSTS.Build.IntegrationBuild", workItemModel.IntegratedInBuild);
            }
        }

        public static string DecodePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('>', '/').Replace('<', '\\');
        }

        public static string EncodePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('/', '>').Replace('\\', '<');
        }

        private static string GetPriorityDescription(int priority)
        {
            switch (priority)
            {
                case 1: return "High";
                case 2: return "Medium";
                default: 
                    return "Low";
            }
        }

        private static string GetFieldValueByReference(this TeamFoundation.WorkItemTracking.Client.FieldCollection fields, string referenceName)
        {
            var field = fields
                            .Cast<TeamFoundation.WorkItemTracking.Client.Field>()
                            .SingleOrDefault(f => f.ReferenceName.Equals(referenceName, StringComparison.OrdinalIgnoreCase));

            return (field != null) && (field.Value != null)
                ? field.Value.ToString()
                : null;
        }

        private static void SetFieldValueByReference(this TeamFoundation.WorkItemTracking.Client.FieldCollection fields, string referenceName, object value)
        {
            var field = fields
                            .Cast<TeamFoundation.WorkItemTracking.Client.Field>()
                            .SingleOrDefault(f => f.ReferenceName.Equals(referenceName, StringComparison.OrdinalIgnoreCase));

            if (field != null)
            {
                field.Value = value;
            }            
        }
    }
}
