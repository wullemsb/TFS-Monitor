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
    /// There are no comments for Ordina.TFSMonitor.Model.Entities.Project in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Name
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Projects")]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Name", System.Data.Services.Common.SyndicationItemProperty.Title, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Name")]
    public partial class Project : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new Project object.
        /// </summary>
        /// <param name="name">Initial value of Name.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Project CreateProject(string name)
        {
            Project project = new Project();
            project.Name = name;
            return project;
        }
        /// <summary>
        /// There are no comments for Property Name in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this.OnNameChanging(value);
                this._Name = value;
                this.OnNameChanged();
                this.OnPropertyChanged("Name");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Name;
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        /// <summary>
        /// There are no comments for Property Collection in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Collection
        {
            get
            {
                return this._Collection;
            }
            set
            {
                this.OnCollectionChanging(value);
                this._Collection = value;
                this.OnCollectionChanged();
                this.OnPropertyChanged("Collection");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Collection;
        partial void OnCollectionChanging(string value);
        partial void OnCollectionChanged();
        /// <summary>
        /// There are no comments for Changesets in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<Changeset> Changesets
        {
            get
            {
                return this._Changesets;
            }
            set
            {
                this._Changesets = value;
                this.OnPropertyChanged("Changesets");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Changeset> _Changesets = new global::System.Data.Services.Client.DataServiceCollection<Changeset>(null, System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// There are no comments for Builds in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<Build> Builds
        {
            get
            {
                return this._Builds;
            }
            set
            {
                this._Builds = value;
                this.OnPropertyChanged("Builds");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Build> _Builds = new global::System.Data.Services.Client.DataServiceCollection<Build>(null, System.Data.Services.Client.TrackingMode.None);
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
        /// <summary>
        /// There are no comments for Queries in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<Query> Queries
        {
            get
            {
                return this._Queries;
            }
            set
            {
                this._Queries = value;
                this.OnPropertyChanged("Queries");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Query> _Queries = new global::System.Data.Services.Client.DataServiceCollection<Query>(null, System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// There are no comments for Branches in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<Branch> Branches
        {
            get
            {
                return this._Branches;
            }
            set
            {
                this._Branches = value;
                this.OnPropertyChanged("Branches");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<Branch> _Branches = new global::System.Data.Services.Client.DataServiceCollection<Branch>(null, System.Data.Services.Client.TrackingMode.None);
        /// <summary>
        /// There are no comments for AreaPaths in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<AreaPath> AreaPaths
        {
            get
            {
                return this._AreaPaths;
            }
            set
            {
                this._AreaPaths = value;
                this.OnPropertyChanged("AreaPaths");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<AreaPath> _AreaPaths = new global::System.Data.Services.Client.DataServiceCollection<AreaPath>(null, System.Data.Services.Client.TrackingMode.None);
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
