﻿// ----------------------------------------------------------------------------------
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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Original file name: TFSModel.cs
// Generation date: 03/11/2010 08:13:31 p.m.
namespace Ordina.TFSMonitor.Model.Entities
{
    /// <summary>
    /// There are no comments for Ordina.TFSMonitor.Model.Entities.Changeset in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Id
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Changesets")]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("ArtifactUri", System.Data.Services.Common.SyndicationItemProperty.Title, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Comment", System.Data.Services.Common.SyndicationItemProperty.Summary, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("CreationDate", System.Data.Services.Common.SyndicationItemProperty.Updated, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Id")]
    public partial class Changeset : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new Changeset object.
        /// </summary>
        /// <param name="ID">Initial value of Id.</param>
        /// <param name="creationDate">Initial value of CreationDate.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Changeset CreateChangeset(int ID, global::System.DateTime creationDate)
        {
            Changeset changeset = new Changeset();
            changeset.Id = ID;
            changeset.CreationDate = creationDate;
            return changeset;
        }
        /// <summary>
        /// There are no comments for Property Id in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                this.OnIdChanging(value);
                this._Id = value;
                this.OnIdChanged();
                this.OnPropertyChanged("Id");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _Id;
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        /// <summary>
        /// There are no comments for Property ArtifactUri in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string ArtifactUri
        {
            get
            {
                return this._ArtifactUri;
            }
            set
            {
                this.OnArtifactUriChanging(value);
                this._ArtifactUri = value;
                this.OnArtifactUriChanged();
                this.OnPropertyChanged("ArtifactUri");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _ArtifactUri;
        partial void OnArtifactUriChanging(string value);
        partial void OnArtifactUriChanged();
        /// <summary>
        /// There are no comments for Property Comment in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this.OnCommentChanging(value);
                this._Comment = value;
                this.OnCommentChanged();
                this.OnPropertyChanged("Comment");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Comment;
        partial void OnCommentChanging(string value);
        partial void OnCommentChanged();
        /// <summary>
        /// There are no comments for Property Committer in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Committer
        {
            get
            {
                return this._Committer;
            }
            set
            {
                this.OnCommitterChanging(value);
                this._Committer = value;
                this.OnCommitterChanged();
                this.OnPropertyChanged("Committer");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Committer;
        partial void OnCommitterChanging(string value);
        partial void OnCommitterChanged();
        /// <summary>
        /// There are no comments for Property CreationDate in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime CreationDate
        {
            get
            {
                return this._CreationDate;
            }
            set
            {
                this.OnCreationDateChanging(value);
                this._CreationDate = value;
                this.OnCreationDateChanged();
                this.OnPropertyChanged("CreationDate");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _CreationDate;
        partial void OnCreationDateChanging(global::System.DateTime value);
        partial void OnCreationDateChanged();
        /// <summary>
        /// There are no comments for Property Owner in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Owner
        {
            get
            {
                return this._Owner;
            }
            set
            {
                this.OnOwnerChanging(value);
                this._Owner = value;
                this.OnOwnerChanged();
                this.OnPropertyChanged("Owner");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Owner;
        partial void OnOwnerChanging(string value);
        partial void OnOwnerChanged();
        /// <summary>
        /// There are no comments for Property Branch in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Branch
        {
            get
            {
                return this._Branch;
            }
            set
            {
                this.OnBranchChanging(value);
                this._Branch = value;
                this.OnBranchChanged();
                this.OnPropertyChanged("Branch");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Branch;
        partial void OnBranchChanging(string value);
        partial void OnBranchChanged();
        /// <summary>
        /// There are no comments for Property WebEditorUrl in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string WebEditorUrl
        {
            get
            {
                return this._WebEditorUrl;
            }
            set
            {
                this.OnWebEditorUrlChanging(value);
                this._WebEditorUrl = value;
                this.OnWebEditorUrlChanged();
                this.OnPropertyChanged("WebEditorUrl");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _WebEditorUrl;
        partial void OnWebEditorUrlChanging(string value);
        partial void OnWebEditorUrlChanged();
        /// <summary>
        /// There are no comments for Changes in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<Change> Changes
        {
            get
            {
                return this._Changes;
            }
            set
            {
                this._Changes = value;
                this.OnPropertyChanged("Changes");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Change> _Changes = new global::System.Data.Services.Client.DataServiceCollection<Change>(null, System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// There are no comments for WorkItems in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<WorkItem> WorkItems
        {
            get
            {
                return this._WorkItems;
            }
            set
            {
                this._WorkItems = value;
                this.OnPropertyChanged("WorkItems");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<WorkItem> _WorkItems = new global::System.Data.Services.Client.DataServiceCollection<WorkItem>(null, System.Data.Services.Client.TrackingMode.None);
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
}
