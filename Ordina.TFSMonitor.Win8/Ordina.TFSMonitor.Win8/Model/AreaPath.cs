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
    /// There are no comments for Ordina.TFSMonitor.Model.Entities.AreaPath in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Path
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("AreaPaths")]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Path", System.Data.Services.Common.SyndicationItemProperty.Title, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.EntityPropertyMappingAttribute("Name", System.Data.Services.Common.SyndicationItemProperty.Summary, System.Data.Services.Common.SyndicationTextContentKind.Plaintext, true)]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("Path")]
    public partial class AreaPath : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new AreaPath object.
        /// </summary>
        /// <param name="path">Initial value of Path.</param>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static AreaPath CreateAreaPath(string path)
        {
            AreaPath areaPath = new AreaPath();
            areaPath.Path = path;
            return areaPath;
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
        /// There are no comments for SubAreas in the schema.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceCollection<AreaPath> SubAreas
        {
            get
            {
                return this._SubAreas;
            }
            set
            {
                this._SubAreas = value;
                this.OnPropertyChanged("SubAreas");
            }
        }
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceCollection<AreaPath> _SubAreas = new global::System.Data.Services.Client.DataServiceCollection<AreaPath>(null, System.Data.Services.Client.TrackingMode.None);
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
