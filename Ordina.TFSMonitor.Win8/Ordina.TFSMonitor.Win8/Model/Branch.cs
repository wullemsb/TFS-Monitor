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
    /// There are no comments for Ordina.TFSMonitor.Model.Entities.Branch in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Path
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("Branches")]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Path", System.Data.Services.Common.SyndicationItemProperty.Title, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Description", System.Data.Services.Common.SyndicationItemProperty.Summary, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("DateCreated", System.Data.Services.Common.SyndicationItemProperty.Updated, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Path")]
    public partial class Branch : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new Branch object.
        /// </summary>
        /// <param name="path">Initial value of Path.</param>
        /// <param name="dateCreated">Initial value of DateCreated.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Branch CreateBranch(string path, global::System.DateTime dateCreated)
        {
            Branch branch = new Branch();
            branch.Path = path;
            branch.DateCreated = dateCreated;
            return branch;
        }
        /// <summary>
        /// There are no comments for Property Path in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Path
        {
            get
            {
                return this._Path;
            }
            set
            {
                this.OnPathChanging(value);
                this._Path = value;
                this.OnPathChanged();
                this.OnPropertyChanged("Path");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Path;
        partial void OnPathChanging(string value);
        partial void OnPathChanged();
        /// <summary>
        /// There are no comments for Property Description in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this.OnDescriptionChanging(value);
                this._Description = value;
                this.OnDescriptionChanged();
                this.OnPropertyChanged("Description");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Description;
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        /// <summary>
        /// There are no comments for Property DateCreated in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime DateCreated
        {
            get
            {
                return this._DateCreated;
            }
            set
            {
                this.OnDateCreatedChanging(value);
                this._DateCreated = value;
                this.OnDateCreatedChanged();
                this.OnPropertyChanged("DateCreated");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _DateCreated;
        partial void OnDateCreatedChanging(global::System.DateTime value);
        partial void OnDateCreatedChanged();
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
