<%@ Page Language="C#" AutoEventWireup="false" Inherits="System.Web.UI.Page" ViewStateEncryptionMode="Never" ViewStateMode="Disabled" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
    <title>CodePlex OData API</title>
    <meta http-equiv="Content-type" content="application/xhtml+xml" />
    <meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />
    <meta content="en" name="language" />
    <meta content="en" http-equiv="Content-Language" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="X-XSS-Protection" content="0" />
    <link rel="stylesheet" href="<%= this.ResolveUrl("~/site.css") %>" type="text/css" media="screen" charset="utf-8" />
    </head>
<body>
    <% 
        var baseUrl = this.Context.Request.Url.GetComponents(UriComponents.SchemeAndServer & ~UriComponents.Port, UriFormat.UriEscaped).ToString();
    %>
    <div id="container">
       <div id="header">
            <h1 style="float: left; margin-top:40px; padding: 5px 0px">CodePlex OData API</h1>
            <a title="Open Data Proptocol OData" href="http://www.odata.org/" class="odata-logo">
                <img height="40" width="142" alt="OData" src="<%= this.ResolveUrl("~/OData-logo.png") %>" />
                </a>
            <a title="CodePlex" href="http://www.codeplex.com/" class="codeplex-logo">
                <img height="100" width="100" alt="CodePlex" src="<%= this.ResolveUrl("~/CodePlex-logo.png") %>" />
                </a>
            <div class="header-clearer">
            </div>
        </div>
        <div id="content">
            <h2>Overview</h2>

            <p>The CodePlex OData API is an implementation of the OData protocol built upon the existing Team Foundation Server 2010 API used to connect to CodePlex. The API is subject to change as we get feedback from customers.</p>         
                        
            <p>To learn more about the OData protocol, you can browse the OData site at <a title="Open Data Proptocol OData" href="http://www.odata.org/" class="odata-logo">http://www.odata.org</a>.</p>

            <p>If you have questions or feedback about this service, please email <a href="mailto:TFSOData@Microsoft.com">TFSOData@Microsoft.com</a>. Please note that this service is provided &quot;as-is&quot;, with no guaranteed uptime and is not officially supported by Microsoft. But if you are having problems please let us know and we&#39;ll do our best to work with you.</p>

            <h3>See the Demo</h3>
            <p>There is a <a href="http://channel9.msdn.com/Blogs/briankel/OData-Service-for-Team-Foundation-Server-2010">video for Channel 9</a> which shows how to get started using the service.</p>

            <h3>Download the Demo</h3>
            <p><a href="http://www.microsoft.com/downloads/details.aspx?FamilyID=d6f8968c-f27f-43fb-88ae-8805db257a67">Download</a> the demo which includes the source code and the full documentation. The service can be easily hosted in Windows Azure to front-end your own Team Foundation Server instance.</p>

            <h3>Getting Started</h3>
            <p>In the following section you will find meaningful information about how to consume data from the CodePlex TFS taking advantage of the CodePlex OData API.</p>

            <p>While reviewing the available operations you will notice that you need to include a valid TFS Project Collection name as part of the URL. 
                For example, if you want to target the TFS Project Collection named <strong>TFS18</strong> 
                you need to use browse <a title="Builds" href="<%= baseUrl %>/TFS18"><%= baseUrl %>/TFS18</a>.</p>

            <p>To find out the right TFS Project Collection for a specific CodePlex project follow these steps:</p>
            <ul>
                <li>Browse to a <a href="http://www.codeplex.com">CodePlex</a> project web site where you have a 
                    <em>Developer</em> or <em>Coordinator</em> role.</li>
                <li>Sign in using your CodePlex credentials.</li>
                <li>Click in the <strong>Source Control</strong> tab.</li>
                <li>In the right page, click the <strong>Visual Studio Team Explorer</strong> link inside the <strong>Source Control Setup</strong> section.</li>
                <li>
                    <span>Take note of the <strong>Project Collection</strong> name from the emerging pane.</span>
                    <img height="388" width="484" src="<%= this.ResolveUrl("~/VisualStudioTeamExplorerSettings.png") %>" alt="Visual Studio Team Explorer Settings" />
                      <span>The CodePlex username used to login to this service must also be suffixed with <strong>_cp</strong>.</span>
                </li>
            </ul>

            <h4>Collections</h4>
            <p>The main resources available are Builds, Changesets, Changes, 
                Builds, Builddd Definitions, Branches, Work Items, Attachments, Projects, Queries and Area Paths. A couple of sample queries are provided for each resource, although complete query options are provided further in this 
                page.</p>

            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Resources</th>
                    <th>Path</th>
                </tr>
                <tr>
                    <td>Builds</td>
                    <td>
                        <a title="Builds" href="<%= baseUrl %>/TFSCollectionName/Builds"><%= baseUrl %>/TFSCollectionName/Builds</a><br /><br />
                        <a title="Builds" href="<%= baseUrl %>/TFSCollectionName/Projects('projectName')/Builds"><%= baseUrl %>/TFSCollectionName/Projects(&#39;projectName&#39;)/Builds</a><br /><br />
                    </td>
                </tr>
                <tr>
                    <td>Build Definitions</td>
                    <td>
                        <a title="Build Definitions" href="<%= baseUrl %>/TFSCollectionName/Projects('projectName')/BuildDefinitions"><%= baseUrl %>/TFSCollectionName/Projects(&#39;projectName&#39;)/BuildDefinitions</a><br /><br />
                    </td>
                </tr>
                <tr>
                    <td>Changesets</td>
                    <td>
                    <a title="Changesets" href="<%= baseUrl %>/TFSCollectionName/Changesets"><%= baseUrl %>/TFSCollectionName/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/TFSCollectionName/Projects('projectName')/Changesets"><%= baseUrl %>/TFSCollectionName/Projects(&#39;projectName&#39;)/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/TFSCollectionName/Branches('path')/Changesets"><%= baseUrl %>/TFSCollectionName/Branches(&#39;path&#39;)/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/TFSCollectionName/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')/Changesets"><%= baseUrl %>/TFSCollectionName/Builds(Project=&#39;prjName&#39;,Definition=&#39;BuildDef&#39;,Number=&#39;BuildNum&#39;)/Changesets</a></td>
                </tr> 
                <tr>
                    <td>Changes</td>
                    <td>
                        <a title="Changes" href="<%= baseUrl %>/TFSCollectionName/Changesets(10)/Changes"><%= baseUrl %>/TFSCollectionName/Changesets(Id)/Changes</a>
                    </td>
                </tr>

                <tr>
                    <td>Branches</td>
                    <td>
                        <a title="Branches" href="<%= baseUrl %>/TFSCollectionName/Branches"><%= baseUrl %>/TFSCollectionName/Branches</a><br /><br />
                        <a title="Branches" href="<%= baseUrl %>/TFSCollectionName/Projects('prjName')/Branches"><%= baseUrl %>/TFSCollectionName/Projects(&#39;prjName&#39;)/Branches</a>
                    </td>
                </tr>   

                <tr>
                    <td>WorkItems</td>
                    <td>
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/WorkItems"><%= baseUrl %>/TFSCollectionName/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')/WorkItems"><%= baseUrl %>/TFSCollectionName/Builds(Project=&#39;prjName&#39;,Definition=&#39;BuildDef&#39;,Number=&#39;BuildNum&#39;)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/Changesets(10)/WorkItems"><%= baseUrl %>/TFSCollectionName/Changesets(id)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/Projects('projectName')/WorkItems"><%= baseUrl %>/TFSCollectionName/Projects(&#39;projectName&#39;)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/Queries('id')/WorkItems"><%= baseUrl %>/TFSCollectionName/Queries(&#39;id&#39;)/WorkItems</a>
                    </td>
                </tr>

                <tr>
                    <td>Attachments</td>
                    <td>
                        <a title="Attachments" href="<%= baseUrl %>/TFSCollectionName/WorkItems(10)/Attachments"><%= baseUrl %>/TFSCollectionName/WorkItems(id)/Attachments</a>
                    </td>
                </tr>

                <tr>
                    <td>Projects</td>
                    <td>
                        <a title="Projects" href="<%= baseUrl %>/TFSCollectionName/Projects"><%= baseUrl %>/TFSCollectionName/Projects</a>
                    </td>
                </tr>

                <tr>
                    <td>Queries</td>
                    <td>
                        <a title="Queries" href="<%= baseUrl %>/TFSCollectionName/Queries"><%= baseUrl %>/TFSCollectionName/Queries</a>
                    </td>
                </tr>

                <tr>
                    <td>AreaPaths</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/TFSCollectionName/AreaPaths"><%= baseUrl %>/TFSCollectionName/AreaPaths</a><br /><br />
                        <a title="AreaPaths" href="<%= baseUrl %>/TFSCollectionName/Projects('projectName')/AreaPaths"><%= baseUrl %>/TFSCollectionName/Projects('projectName')/AreaPaths</a>
                    </td>
                </tr>
            </table>

            <h4>Individual Resources</h4>
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Resource</th>
                    <th>Path</th>
                    <th>Related Resources</th>
                    <th>Fields *</th>		
                </tr>
                <tr>
                    <td>Build</td>
                    <td>
                        <a title="Build" href="<%= baseUrl %>/TFSCollectionName/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')"><%= baseUrl %>/TFSCollectionName/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')</a>
                    </td>
                    <td>
                        WorkItems, Changesets</td>
                    <td>
                        <strong>Project</strong>, <strong>Definition</strong>,<strong> Number</strong>, Reason, Quality, Status, RequestedBy, 
                        RequestedFor, LastChangedBy, StartTime, FinishTime, LastChangedOn, 
                        BuildFinishied, DropLocation, Errors, Warnings</td>
                </tr>
                <tr>
                    <td>Build Definition</td>
                    <td>
                        <a title="Build Definition" href="<%= baseUrl %>/TFSCollectionName/BuildDefinitions(Project='prjName',Definition='BuildDef')"><%= baseUrl %>/TFSCollectionName/BuildDefinitions(Project='prjName',Definition='BuildDef')</a>
                    </td>
                    <td>
                        -</td>
                    <td>
                        <strong>Project</strong>, <strong>Definition</strong></td>
                </tr>
                <tr>
                    <td>Changeset</td>
                    <td>
                        <a title="Changeset" href="<%= baseUrl %>/TFSCollectionName/Changesets(10)"><%= baseUrl %>/TFSCollectionName/Changesets(id)</a>
                    <td>Changes, WorkItems</td>
                    <td><strong>Id</strong>, ArtifactUri, Comment, Committer, CreationDate, Owner, Branch, WebEditorUrl</td>
                </tr> 
                <tr>
                    <td>Change</td>
                    <td>
                        <a title="Change" href="<%= baseUrl %>/TFSCollectionName/Changes(Changeset='id',Path='path')"><%= baseUrl %>/TFSCollectionName/Changes(Changeset=&#39;id&#39;,Path=&#39;path&#39;)</a>
                    </td>
                    <td>-</td>
                    <td><strong>Changeset</strong>, <strong>Path</strong>, Collection, ChangeType, Type</td>
                </tr>

                <tr>
                    <td>Branch</td>
                    <td>
                        <a title="Branch" href="<%= baseUrl %>/TFSCollectionName/Branches('path')"><%= baseUrl %>/TFSCollectionName/Branches(&#39;path&#39;)</a>
                    </td>
                    <td>Changesets</td>
                    <td><strong>Path</strong>, Description, DateCreated</td>
                </tr>   

                <tr>
                    <td>WorkItem</td>
                    <td>
                        <a title="WorkItem" href="<%= baseUrl %>/TFSCollectionName/WorkItems(10)"><%= baseUrl %>/TFSCollectionName/WorkItems(id)</a>
                    </td>
                    <td>Attachments</td>
                    <td><strong>Id</strong>, AreaPath, IterationPath, Revision, Priority, Severity, StackRank, Project, 
                        AssignedTo, CreatedDate, CreatedBy, ChangedDate, ChangedBy, ResolvedBy, Title, 
                        State, Type, Reason, Description, ReproSteps, FoundInBuild, IntegratedInBuild, 
                        WebEditorUrl</td>
                </tr>

                <tr>
                    <td>Attachment</td>
                    <td>
                        <a title="Attachment" href="<%= baseUrl %>/TFSCollectionName/Attachments('WorkItemId-Index')"><%= baseUrl %>/TFSCollectionName/Attachments(&#39;WorkItemId-Index&#39;)</a>
                    </td>
                    <td>-</td>
                    <td>Id, WorkItemId, Index, AttachedTime, CreationTime, LastWriteTime, Name, 
                        Extension, Comment, Length, Uri</td>
                </tr>

                <tr>
                    <td>Project</td>
                    <td>
                        <a title="Project" href="<%= baseUrl %>/TFSCollectionName/Projects('name')"><%= baseUrl %>/TFSCollectionName/Projects(&#39;name&#39;)</a>
                    </td>
                    <td>Changesets, Builds, BuildDefinitions, WorkItems, Queries, Branches, AreaPaths</td>
                    <td><strong>Name</strong>, Collection</td>
                </tr>

                <tr>
                    <td>Query</td>
                    <td>
                        <a title="Query" href="<%= baseUrl %>/TFSCollectionName/Queries('id')"><%= baseUrl %>/TFSCollectionName/Queries(&#39;id&#39;)</a>
                    </td>
                    <td>WorkItems</td>
                    <td><strong>Id</strong>, Name, Description, QueryText, Path, Project, QueryType</td>
                </tr>

                <tr>
                    <td>AreaPath</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/TFSCollectionName/AreaPaths('path')"><%= baseUrl %>/TFSCollectionName/AreaPaths(&#39;path&#39;)</a><br /><br />
                    </td>
                    <td>SubAreas</td>
                    <td><strong>Path</strong>, Name</td>
                </tr>
            </table>
            <p class="note">* Id fields are displayed in <strong>bold</strong>.</p>
            
            <h4>Parameters Support</h4>
            <p>These are the allowed parameters for manipulating the data that comes out from the OData Service, due to the nature of the API 
                <em>$inlinecount</em> and <em>$expand</em> are not currently supported for this service.</p>
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Values</th>
                    <th>Example</th>		
                </tr>
                <tr>
                    <td>filter</td>
                    <td>Filter the results</td>
                    <td>See filtering support section</td>
                    <td><a title="filter" href="<%= baseUrl %>/TFSCollectionName/Projects?$filter=Name%20eq%20'MyProject'"><%= baseUrl %>/TFSCollectionName/Projects?$filter=Name eq &#39;MyProject&#39;</a><br /></td>
                </tr>
                <tr>
                    <td>count</td>
                    <td>Count the results</td>
                    <td>-</td>
                    <td><a title="count" href="<%= baseUrl %>/TFSCollectionName/WorkItems/$count"><%= baseUrl %>/TFSCollectionName/WorkItems/$count</a><br /></td>
                </tr>
                <tr>
                    <td>top/skip</td>
                    <td>Paging options</td>
                    <td>Integer values</td>
                    <td><a title="top/skip" href="<%= baseUrl %>/TFSCollectionName/WorkItems?$top=5&$skip=10"><%= baseUrl %>/TFSCollectionName/WorkItems?$top=5&amp;$skip=10</a><br /></td>
                </tr>
                <tr>
                    <td>orderby</td>
                    <td>Sort results</td>
                    <td>Resource field</td>
                    <td><a title="orderby" href="<%= baseUrl %>/TFSCollectionName/Builds?$orderby=Reason%20asc"><%= baseUrl %>/TFSCollectionName/Builds?$orderby=Reason asc</a><br /></td>
                </tr>
                <tr>
                    <td>select</td>
                    <td>Fields to return</td>
                    <td>Fields for the resource type</td>
                    <td><a title="select" href="<%= baseUrl %>/TFSCollectionName/Changesets?$select=Committer,Owner"><%= baseUrl %>/TFSCollectionName/Changesets?$select=Committer,Owner</a><br /></td>
                </tr>
            </table>

            <h4>Filtering Support</h4>
            <p>These are the supported fields and operations while filtering out the data that comes out from the 
                service. All these items along with its corresponding operators can be used for filtering data.</p> 
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Entity</th>
                    <th>Supported Filter Operations</th>
                    <th>Example</th>		
                </tr>
                <tr>
                    <td>Build</td>
                    <td>
                        <ul>
                        <li>Users can filter for a specific value (<strong>eq</strong> operator) by Project, Definition, Number, Reason, Quality, Status and RequestedFor.</li>
                        <li>Users can filter for a different value (<strong>ne</strong> operator) and range (<strong>eq</strong>, 
                            <strong>gt</strong>, <strong>lt</strong> operators) by RequestedBy, StartTime, FinishTime and BuildFinished.</li>
                        <li>Only logical <strong>And</strong> operator is supported.</li>
                        </ul>
                    </td>
                    <td><a title="Builds" href="<%= baseUrl %>/TFSCollectionName/Builds?$filter=Definition%20eq%20'buildDef'%20and%20RequestedBy%20ne%20'johndoe'"><%= baseUrl %>/TFSCollectionName/Builds?$filter=Definition eq &#39;buildDef&#39; and RequestedBy ne &#39;johndoe&#39;</a></td>
                </tr>
                <tr>
                    <td>Changeset</td>
                    <td>
                        <ul>
                            <li>Users can filter for a specific value (<strong>eq</strong> operator) by Committer and for a range of values (<strong>eq</strong>, 
                                <strong>gt</strong> and <strong>lt</strong> operators) by CreationDate.</li>
                            <li>Users can filter for a specific and a different value (<strong>eq</strong> and 
                                <strong>ne</strong> operators) by ArtifactUri, Comment and Owner.</li>
                            <li>Only logical <strong>And</strong> operator is supported.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="Builds" href="<%= baseUrl %>/TFSCollectionName/Changesets?$filter=Committer%20eq%20'johndoe'%20and%20ArtifactUri%20ne%20'https://tfsserver/artifact'"><%= baseUrl %>/TFSCollectionName/Changesets?$filter=Committer eq &#39;johndoe&#39; and ArtifactUri ne &#39;https://tfsserver/artifact&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Change</td>
                    <td>
                        <ul>
                            <li>Change collections are only browsable relative to a Changeset.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="Changes" href="<%= baseUrl %>/TFSCollectionName/Changesets(10)/Changes?$filter=Type%20eq%20'file'"><%= baseUrl %>/TFSCollectionName/Changesets(10)/Changes?$filter=Type eq &#39;file&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Branch</td>
                    <td>
                    <ul>
                        <li>Users can filter for a specific value (<strong>eq</strong> operator) by Committer and for a range of values (<strong>eq</strong>, 
                            <strong>gt</strong> and <strong>lt</strong> operators) by CreationDate.</li>
                        <li>Users can filter for a specific and a different value (<strong>eq</strong> and 
                            <strong>ne</strong> operators) by ArtifactUri, Comment and Owner.</li>
                        <li>Only logical <strong>And</strong> operator is supported.</li>
                    </ul>
                    </td>
                    <td>
                        <a title="Branches" href="<%= baseUrl %>/TFSCollectionName/Branches?$filter=Description%20eq%20'Release Branch'"><%= baseUrl %>/TFSCollectionName/Branches?$filter=Description eq &#39;Release Branch&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>WorkItem</td>
                    <td>
                        <ul>
                        <li>Users can filter for a different (<strong>ne</strong> operator), a specific (<strong>eq</strong> operator), or a substring of a value (<strong>substringof
                            </strong>operator) by AreaPath, IterationPath, Priority, Severity, StackRank, Project, AssignedTo, CreatedBy, ChangedBy, ResolvedBy, Title, State, Type, Reason, Description, ReproSteps, FoundInBuild and IntegratedInBuild.</li>
                        <li>Users can filter for a different (<strong>ne</strong> operator), a specific (<strong>eq
                            </strong>operator) or a range (<strong>gt</strong>, <strong>lt</strong>, <strong>ge</strong>, 
                            <strong>le</strong> operators) of values by CreatedDate, ChangedDate and Revision.</li>
                        <li>Logical <strong>And</strong> and <strong>Or</strong> operators are supported.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="WorkItems" href="<%= baseUrl %>/TFSCollectionName/WorkItems?$filter=Project%20eq%20'myProject' or substringof('fixed', State)%20eq%20true"><%= baseUrl %>/TFSCollectionName/WorkItems?$filter=Project eq &#39;myProject&#39; or substringof(&#39;fixed&#39;, State) eq true</a>
                    </td>
                </tr>
                <tr>
                    <td>Attachment</td>
                    <td><ul><li>Attachment collections are only browsable relative to a WorkItem.</li></ul></td>
                    <td>
                        <a title="Attachments" href="<%= baseUrl %>/TFSCollectionName/WorkItems(5)/Attachments?$filter=Extension%20eq%20'JPG'"><%= baseUrl %>/TFSCollectionName/WorkItems(5)/Attachments?$filter=Extension eq &#39;JPG&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Project</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="Projects" href="<%= baseUrl %>/TFSCollectionName/Projects?$filter=Name%20eq%20'myProject'"><%= baseUrl %>/TFSCollectionName/Projects?$filter=Name eq &#39;myProject&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Query</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="Queries" href="<%= baseUrl %>/TFSCollectionName/Queries?$filter=Name%20eq%20'All Work Items'"><%= baseUrl %>/TFSCollectionName/Queries?$filter=Name eq &#39;All Work Items&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>AreaPath</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/TFSCollectionName/AreaPaths?$filter=Name%20eq%20'Area 1'"><%= baseUrl %>/TFSCollectionName/AreaPaths?$filter=Name eq &#39;Area 1&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>BuildDefinition</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="BuildDefinitions" href="<%= baseUrl %>/TFSCollectionName/BuildDefinitions?$filter=Project%20eq%20'MyProject'"><%= baseUrl %>/TFSCollectionName/AreaPaths?$filter=Project eq &#39;MyProject&#39;</a>
                    </td>
                </tr>
            </table>

            <h4>Write Operations Support</h4>
            <p>The following entities allow write or update operations as specified by the <a href="http://www.odata.org/developers/protocols/operations#CreatingnewEntries">OData specification</a>.
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Entity</th>
                    <th>Supported Operations</th>
                </tr>
                <tr>
                    <td>WorkItem</td>
                    <td>
                        <ul>
                            <li>Creating an entity through an HTTP POST operation to the main collection.</li>
                            <li>Updating an entity through an HTTP PUT operation to an individual resource.</li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>Attachment</td>
                    <td>
                        <ul>
                            <li>Creating an entity through an HTTP POST operation to the main collection.</li>
                        </ul>
                    </td>
                </tr>
            </table>
<h4>Service Operations Support</h4>
            <p>The following custom operations are supported by the service, as specified by the <a href="http://www.odata.org/developers/protocols/operations#InvokingServiceOperations">OData specification</a>.
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Operation</th>
                    <th>Method</th>
                    <th>Request Body</th>
                    <th>Description</th>
                </tr>
                <tr>
                    <td>TFSCollectionName/TriggerBuild</td>
                    <td>POST</td>
                    <td>
                        Project=<i>prjName</i>&Definition=<i>buildDef</i>
                    </td>
                    <td>
                        Triggers a new build using the Build Definition <i>buildDef</i> belonging to the Team Project 
                        <i>prjName</i> inside the TFS Project Collection named <i>TFSCollectionName</i>.
                    </td>
                </tr>
            </table>
        </div>
        <div id="footer">
            <span>&copy; <%= DateTime.Now.Year %> Microsoft Corporation. All rights reserved.</span>
        </div>
    </div>
</body>
</html>
