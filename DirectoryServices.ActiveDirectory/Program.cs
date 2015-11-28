/*=====================================================================
  File:     Program.cs

  Summary:  The main entry-point to all public classes in this project
=======================================================================
                     
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.IO;

namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
           

            #region no argument specified
            if (args.GetLength(0) == 0)
            {

                //ShowCommandSyntax();
                ShowHelp();
            }
            #endregion
            else
            {
                switch (args[0].ToLower())
                {
                    #region test demonstrations
                    case "getforestdata":
                        FromForest.GetConfigData();
                        break;
                    case "getgcdata":
                        FromForest.GetGlobalCatalogConfigData();
                        break;
                    case "getdomaindata":
                        FromDomain.GetConfigData();
                        break;
                    case "getdcdata":
                        FromDomain.GetDomainControllerConfigData();
                        break;
                    case "getschemadata":
                        SchemaData.GetSchemaData();
                        break;
                    case "getschemaclassdata":
                        try
                        {
                            SchemaData.GetSchemaClassData(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("You must include the name of a schema class object.\n" +
                                "For example, to get information about the user class, type ds.ad GetSchemaClassData user");
                            break;
                        }
                        break;

                    case "getschemapropertydata":
                        try
                        {
                            SchemaData.GetSchemaPropertyData(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("You must include the name of a schema attribute object.\n" +
                                "For example, to get information about the cn attribute, type ds.ad GetSchemaPropertyData cn");
                        }
                        break;

                    case "getadampartitions":
                        AdamData.GetAdamInstanceData();
                        break;

                    case "getadamschemadata":
                        AdamData.GetSchemaData();
                        break;

                    case "getadamschemaclassdata":
                        try
                        {
                            AdamData.GetSchemaClassData(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: GetAdamSchemaClassData className\n\n" +
                                "You must include the name of a schema class object.\n" +
                                "For example, to get information about the user class, type ds.ad GetAdamSchemaClassData user");
                            break;
                        }
                        break;
                    case "getadamschemapropertydata":
                        try
                        {
                            AdamData.GetSchemaPropertyData(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: GetAdamSchemaPropertyData attributeName\n\n" +
                                "You must include the name of a schema attribute object.\n" +
                                "For example, to get information about the cn attribute, type ds.ad GetAdamSchemaPropertyData cn");
                        }
                        break;

                    case "addschemaclasstoadam":
                        ExtendSchema.CreateNewClass();
                        break;

                    case "addschemaattributetoadam":
                        ExtendSchema.CreateNewAttribute();
                        break;

                    case "gettopologydata":
                        try
                        {
                            ADTopology.GetData(args[1]);

                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: GetTopologyData forestName\n\n" +
                            "You must specify the forest name to get topology data.\n" +
                            "For example, to review topology data for the fabrikam.com domain, type:\n" +
                            "ds.ad topologyData fabrikam.com");

                        }
                        break;

                    #region: //topology management actions
                    case "createadsite":
                        try
                        {
                            MngTopology.CreateAdSite(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateAdSite newSiteName\n\n" +
                                "You must include a site name you want to create (e.g., site1");
                        }
                        break;

                    case "createadamsite":
                        try
                        {
                            MngTopology.CreateAdamSite(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateAdamSite targetName newSiteName\n\n" +
                            "You must include an ADAM server name:port value as the target name (e.g., Server1:50000),\n" +
                            "and the site name you want to create (e.g., site1");
                        }
                        break;

                    case "createsubnet":
                        try
                        {
                            MngTopology.CreateSubnet(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateSubnet newSubnet subnetLocationName siteName\n\n" +
                            "You must include the subnet you want to create (e.g., 10.1.1.0/24),\n" +
                            "a location for the subnet (e.g., \"Building 1\")\n" +
                            "and the site name to which you want this subnet assigned (e.g., site1)");
                        }
                        break;

                    case "createsubnetplus":
                        try
                        {
                            MngTopology.CreateSubnetPlus(args[1], args[2], args[3],args[4]);
                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: CreateSubnet newSubnet subnetLocationName siteName\n\n" +
                            "You must include the subnet you want to create (e.g., 10.1.1.0/24),\n" +
                            "a location for the subnet (e.g., \"Building 1\")\n" +
                            "and the site name to which you want this subnet assigned (e.g., site1)");
                            Console.ResetColor();
                        }
                        break;

                    case "findsubnet":

                        try
                        {
                            //MngTopology.FindSubnet(args[1]);
                            MngTopology.FindSubnet(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: FindSubnet Forest + Subnet");
                            Console.ResetColor();
                        }
                        break;

                    case "findsubnetbyloc":

                        try
                        {
                            //MngTopology.FindSubnet(args[1]);
                            MngTopology.FindSubnetByLoc(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: FindSubnet Forest + SubnetLocation");
                            Console.ResetColor();
                        }
                        break;
                    case "modifysubnetdescriptionnof":

                        try
                        {
                            MngTopology.modifySubnetDescNoFor(args[0], args[1]);
                            //MngTopology.modifySubnetDesc(args[1], args[2], args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifySubnet Subnet + Description");
                            Console.ResetColor();
                        }
                        break;

                    case "modifysubnetdescription":

                        try
                        {
                            // MngTopology.modifySubnetDesc(args[0], args[1]);
                            MngTopology.modifySubnetDesc(args[1], args[2], args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifySubnet Forest + Subnet + Description");
                            Console.ResetColor();
                        }
                        break;

                    case "modifysubnetlocation":

                        try
                        {
                            // MngTopology.modifySubnetDesc(args[0], args[1]);
                            MngTopology.modifySubnetLoc(args[1], args[2], args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifySubnet Forest + Subnet + Location");
                            Console.ResetColor();
                        }
                        break;


                    case "modifysubnetlocation2":

                        try
                        {
                            // MngTopology.modifySubnetDesc(args[0], args[1]);
                            MngTopology.modifySubnetLoc2(args[1], args[2], args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifySubnet Forest + Subnet + Location");
                            Console.ResetColor();
                        }
                        break;

                    case "modifysitedesc":

                        try
                        {
                            // MngTopology.modifySubnetDesc(args[0], args[1]);
                            MngTopology.ModifySiteDesc(args[1], args[2], args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifysitedesc Forest + Site + Description");
                            Console.ResetColor();
                        }
                        break;

                    case "deletesubnet":

                        try
                        {
                            // MngTopology.DeleteSubnet(args[1]);
                            MngTopology.DeleteSubnet(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: modifySubnet Forest + Subnet + Description");
                            Console.ResetColor();
                        }
                        break;

                    case "findsitelink":

                        try
                        {
                            MngTopology.FindSiteLink(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: findsite Forest + SiteName");
                            Console.ResetColor();
                        }
                        break;

                    case "findsitelink2":

                        try
                        {
                            MngTopology.FindSiteLink2(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: findsite Forest + SiteName");
                            Console.ResetColor();
                        }
                        break;

                    case "findsite":

                        try
                        {
                            MngTopology.FindSite(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: findsitelink Forest + SiteLinkName");
                            Console.ResetColor();
                        }
                        break;

                    case "findsite2":

                        try
                        {
                            MngTopology.FindSite2(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: findsitelink Forest + SiteLinkName");
                            Console.ResetColor();
                        }
                        break;

                    case "modifysldesc":

                        try
                        {
                            MngTopology.ModifySLDesc(args[1], args[2],args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: ModifySiteLinkDesc Forest + SiteLinkName + SiteLinkDescription");
                            Console.ResetColor();
                        }
                        break;

                    case "modifyslname":

                        try
                        {
                            MngTopology.ModifySLName(args[1], args[2],args[3]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: ModifySiteLinkDesc Forest + SiteLinkName + SiteLinkDescription");
                            Console.ResetColor();
                        }
                        break;

                    //// MoveDcToSite

                    case "movedctosite":

                         try
                        {
                            MngTopology.MoveDcToSite(args[1], args[2]);

                        }
                        catch (IndexOutOfRangeException)
                        {

                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The syntax is: MoveDcToSite SourceDC + TargetSite");
                            Console.ResetColor();
                        }
                        break;

                        /*
                    case "createsubnetFCSV":
                        try
                        {

                            string contents = String.Empty;
                            using (FileStream fs = File.Open(@"C:\code\C#\DirectoryServices.ActiveDirectory\bin\Release\Test.csv", FileMode.Open))
                            using (StreamReader reader = new StreamReader(fs))
                            {
                                contents = reader.ReadToEnd();
                            }

                            if (contents.Length > 0)
                            {
                                string[] lines = contents.Split(new char[] { '\n' });
                                
                                //Dictionary<string, string> mysettings = new Dictionary<string, string>();
                                foreach (string line in lines)
                                {
                                    string[] keyAndValue = line.Split(new char[] { '=' });
                                    Console.WriteLine(keyAndValue);
                                    //mysettings.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                                }

                                //string test = mysettings["USERID"]; // example of getting userid
                            }

                            MngTopology.CreateSubnetFromCSV(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateSubnet newSubnet subnetLocationName siteName\n\n" +
                            "You must include the subnet you want to create (e.g., 10.1.1.0/24),\n" +
                            "a location for the subnet (e.g., \"Building 1\")\n" +
                            "and the site name to which you want this subnet assigned (e.g., site1)");
                        }
                        break;

                         */
                         
                    case "createsitelink":
                        try
                        {
                            MngTopology.CreateSiteLink(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateSiteLink siteName siteLinkName\n\n" +
                            "You must include the a site you want to add to the link (i.e., site1),\n" +
                            "and the site link you want to create (e.g., siteLink1)");
                        }
                        break;

                    case "deleteadsite":
                        try
                        {
                            MngTopology.DeleteAdSite(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: DeleteAdSite siteName\n" +
                            "You must include the site name you want to delete (e.g., site1");
                        }
                        break;


                    case "deleteadamsite":
                        try
                        {
                            MngTopology.DeleteAdamSite(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: DeleteAdamSite targetName siteName\n\n" +
                            "You must include a server:port number value as the target name (e.g., server1:50000),\n" +
                            "and the site name you want to delete (e.g., site1)");
                        }

                        break;

                    case "deletelink":
                        try
                        {
                            MngTopology.DeleteLink(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: DeleteLink linkName\n" +
                            "You must include the link name you want to delete (e.g., link1");
                        }
                        break;


                    case "addsitetositelink":
                        try
                        {
                            MngTopology.AddSiteToSiteLink(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: AddSiteToSiteLink siteName siteLinkName\n\n" +
                            "You must include the site name you want to add to the link (e.g., site1" +
                            "and the link name to which the site will be added (e.g., link1)");
                        }
                        break;

                    case "removesitefromsitelink":
                        try
                        {
                            MngTopology.RemoveSiteFromSiteLink(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: RemoveSiteFromSiteLink siteName siteLinkName\n\n" +
                            "You must include the site name you want to remove from the link (e.g., site1" +
                            "and the link name from which the site will be removed (e.g., link1)");
                        }
                        break;

                    #endregion //end topology reporting and management actions

                    #region //replication reporting and management actions

                    case "getreplicationstatedata":
                        ReplicationState.GetData();
                        break;


                    case "replicatefromsource":
                        {
                            try
                            {
                                ReplicationManagement.ReplicateFromSource(args[1], args[2], args[3]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("The syntax is: ReplicateFromSource partitionDN sourceServer targetServer \n\n" +
                                "You must include the source server name (i.e., sea-dc-01.fabrikam.com) \n" +
                                "and the target server name (i.e., sea-dc-02.fabrikam.com) for replication \n" +
                                "and the partition upon which you want to trigger replication:\n" +
                                "(i.e., DC=fabrikam,DC=com)");
                            }
                            break;

                        }

                    case "replicatefromneighbors":
                        {
                            try
                            {
                                ReplicationManagement.ReplicateFromNeighbors(args[1], args[2]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("The syntax is: ReplicateFromNeighbors targetServer partitionDN\n\n" +
                                "You must include the target server name (i.e., sea-dc-02.fabrikam.com)\n" +
                                "and the partition upon which you want to trigger replication:\n" +
                                "(i.e., cn=configuration,DC=fabrikam,DC=com)");
                            }
                            break;

                        }

                    case "syncallservers":
                        {
                            try
                            {
                                ReplicationManagement.SyncAllServers(args[1]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                Console.WriteLine("The syntax is: SyncAllServers partitionDN \n\n" +
                                "You must include the partition upon which you want to perform \n" +
                                "replication management: (i.e., cn=configuration,DC=fabrikam,DC=com)");
                            }
                            break;

                        }

                    case "createnewconnection":
                        try
                        {
                            ReplicationManagement.CreateNewConnection(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateNewConnection sourceServer targetServer connectionName\n\n" +
                            "You must include the source server name  (i.e., sea-dc-02.fabrikam.com),\n" +
                            "the target server name (i.e., sea-dc-01.fabrikam.com) \n" +
                            "and the name of the new connection (i.e., connection01) \n");
                        }
                        break;

                    case "setreplicationconnection":
                        try
                        {
                            ReplicationManagement.SetReplicationConnection(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: SetReplicationConnection server connectionName\n\n" +
                            "You must include the name of the server containing the connection (i.e., sea-dc-02.fabrikam.com),\n" +
                            "and the name of the connection you want to configure (i.e., connection01 \n");
                        }
                        break;

                        case "deletereplicationconnection":
                        try
                        {
                            ReplicationManagement.DeleteReplicationConnection(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: DeleteReplicationConnection server connectionName\n\n" +
                            "You must include the name of the server containing the connection (i.e., sea-dc-02.fabrikam.com),\n" +
                            "and the name of the connection you want to delete (i.e., connection01 \n");
                        }
                        break;

                    #endregion //end replication reporting and management actions

                    #region //trust reporting and management actions

                    case "getcurrentforesttrusts":
                        ADTrust.GetCurrentForestTrusts();
                        break;

                    case "getcurrentdomaintrusts":
                        ADTrust.GetCurrentDomainTrusts();
                        break;

                    case "gettrustwithtargetforest":
                        try
                        {
                            ADTrust.GetTrustWithTargetForest(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: GetTrustWithTargetForest targetForestName\n\n" +
                            "You must include a target forest as the target name (e.g., adatum.com),\n" +
                            "The target forest is a forest other than the current forest context.");
                        }
                        break;

                    case "gettrustwithtargetdomain":
                        try
                        {
                            ADTrust.GetTrustWithTargetDomain(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: GetTrustWithTargetDomain targetDomainName\n\n" +
                            "You must include a target domain as the target name (e.g., corp.adatum.com),\n" +
                            "The target domain is a domain other than the current domain context.");
                        }
                        break;


                    case "createcrossforesttrust":
                        try
                        {
                            ADTrustManagement.CreateCrossForestTrust(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: CreateCrossForestTrust targetForestName userNameTargetForest password\n\n" +
                            "You must include a target forest name (e.g., adatum.com)\n" +
                            "and a user account and password in the target forest with enough permission to establish the trust.\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    case "setforesttrustattributes":
                        try
                        {
                            ADTrustManagement.SetForestTrustAttributes(args[1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: SetForestTrustAttributes targetForestName  \n\n" +
                            "You must include a target forest name (e.g., adatum.com)\n" +
                            "to modify the forest properties." +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    case "changeforesttrusttooutbound":
                        try
                        {
                            ADTrustManagement.ChangeForestTrustToOutbound(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: ChangeForestTrustToOutbound targetForestName userNameTargetForest password  \n\n" +
                            "You must include a target forest name (e.g., adatum.com),\n" +
                            "and a user account and password in the target forest with enough permission to modify the trust.\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    case "addexcludeddomain":
                        try
                        {
                            ADTrustManagement.AddExcludedDomain(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: AddExcludedDomain targetForestName targetDomainName\n\n" +
                            "You must include a target forest name, a target domain name\n" +
                            "(e.g., fabrikam.com corp.adatum.com)\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete this operation."
                            );
                        }
                        break;

                    case "disabledomainnetbiosname":
                        try
                        {
                            ADTrustManagement.DisableDomainNetBIOSName(args[1], args[2]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: DisableDomainNetbiosName targetForestName targetDomainName\n\n" +
                            "You must include a target forest name, a target domain name\n" +
                            "(e.g., fabrikam.com corp.adatum.com)\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete this operation."
                            );
                        }
                        break;

                    case "repairtrust":
                        try
                        {
                            ADTrustManagement.RepairTrust(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: RepairTrust targetForestName userNameTargetForest password  \n\n" +
                            "You must include a target forest name (e.g., adatum.com),\n" +
                            "and a user account and password in the target forest with enough permission to repair the trust.\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    case "removeforesttrust":
                        try
                        {
                            ADTrustManagement.RemoveForestTrust(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: RemoveForestTrust targetForestName userNameTargetForest password  \n\n" +
                            "You must include a target forest name (e.g., adatum.com)\n" +
                            "and a user account and password in the target forest with enough permission to remove the trust.\n" +
                            "Note: This code example assumes that you are connected to the current forest with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    case "removedomaintrust":
                        try
                        {
                            ADTrustManagement.RemoveDomainTrust(args[1], args[2], args[3]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("The syntax is: RemoveDomainTrust targetDomainName userNameTargetDomain password  \n\n" +
                            "You must include a a target domain name (e.g., corp.adatum.com)\n" +
                            "and a user account and password in the target domain with enough permission to remove the trust.\n" +
                            "Note: This code example assumes that you are connected to the current domain with enough permission to\n" +
                            "complete the local side of this operation."
                            );
                        }
                        break;

                    #endregion //end trust data and management actions

                    case "2.":
                        HelpTopology();
                        break;
                    case "1.":
                        HelpGeneral();
                        break;

                    case "3.":
                        HelpReplication();
                        break;

                    case "4.":
                        HelpTrust();
                        break;

                    case "5.":
                        HelpSchema();
                        break;

                    default:
                        //ShowCommandSyntax();
                        ShowHelp();
                        break;
                    #endregion
                }
            }

            return;

        }

        private static void ShowHelp()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1. Forest, Domain, ADAM - type switch HelpGeneral\n2. Topology reporting and management - type switch HelpTopology\n3. Replication reporting and management- type switch HelpReplication \n4. Trust reporting and management tasks - type switch HelpTrust \n5. Schema reporting and management tasks - type switch HelpSchema");
            Console.ResetColor();
        }

        private static void HelpGeneral()
        {
            string[] general = new string[] { 
                "\nGetDomainData", 
                "GetForestData", 
                "GetGcData",
                "GetDcData", 
                "GetSchemaData",
                "GetSchemaClassData className",
                "GetSchemaPropertyData propertyName",
                "GetAdamPartitions",
               };
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n:: Reporting cmdlets for forest, domain and ADAM ::");
            Console.ResetColor();
            foreach (string arg in general)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }
        }

        private static void HelpTopology()
        {
            string[] topology = new string[] { 
                "\nGetTopologyData forestName",
                "CreateAdSite newSiteName",
                "CreateAdamSite targetName newSiteName",
                "CreateSubnet newSubnet siteName",
                "CreateSiteLink siteName newLinkName",
                "AddSiteToSiteLink siteName siteLinkName",
                "RemoveSiteFromSiteLink siteName siteLinkName",
                "DeleteAdSite siteName",
                "DeleteAdamSite targetName siteName",
                "DeleteLink linkName",
            };
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n:: Topology reporting and management cmdlets ::");
            Console.ResetColor();
            foreach (string arg in topology)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }
        }

        public static void HelpReplication()
        {
            string[] replication = new string[] { 
                "\nGetReplicationStateData",
                "ReplicateFromSource partitionDN sourceServer targetServer",
                "ReplicateFromNeighbors targetServer partitionDN",
                "SyncAllServers partitionDN",
                "CreateNewConnection sourceServer targetServer connectionName",
                "SetReplicationConnection server connectionName",
                "DeleteReplicationConnection server connectionName",
            };

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(":: Replication reporting and management cmdlets ::");
            Console.ResetColor();

            foreach (string arg in replication)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }
        }

                public static void HelpTrust()
        {
            string[] trust = new string[] { 
                "\nGetCurrentForestTrusts",
                "GetCurrentDomainTrusts",
                "GetTrustWithTargetForest",
                "GetTrustWithTargetDomain",
                "CreateCrossForestTrust targetForest userNameTargetForest password",
                "SetForestTrustAttributes targetForestName",
                "ChangeForestTrustToOutbound targetForestName userNameTargetForest password",
                "AddExcludedDomain targetForestName targetDomainName",
                "DisableDomainNetbiosName targetForestName targetDomainName",
                "RepairTrust targetForestName userNameTargetForest password",
                "RemoveForestTrust targetForestName userNameTargetForest password",
                "RemoveDomainTrust targetDomainName userNameTargetDomain password"

            };

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n:: Trust reporting and management tasks ::");
            Console.ResetColor();

            foreach (string arg in trust)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }
            
        }


                public static void HelpSchema()
                {

                    string[] schema = new string[] { 
                "\nGetAdamSchemaData",
                "GetAdamSchemaClassData",
                "GetAdamSchemaPropertyData",
                "AddSchemaClasstoAdam",
                "AddSchemaAttributetoAdam",
                

             };

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\n:: Schema management cmdlets ::");
                    Console.ResetColor();

                    foreach (string arg in schema)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        String argString = System.String.Concat(arg);
                        Console.WriteLine(argString);
                        Console.ResetColor();
                    }

                }


            

        /*
        private static void ShowCommandSyntax()
        {

            string[] general = new string[] { 
                
                "\n:: Reporting cmdlets for forest, domain and ADAM ::",
                "GetDomainData", 
                "GetForestData", 
                "GetGcData",
                "GetDcData", 
                "GetSchemaData",
                "GetSchemaClassData className",
                "GetSchemaPropertyData propertyName",
                "GetAdamPartitions",
               };
            
            string[] schema = new string[] { 
                "\n:: Schema management cmdlets ::", 
                "GetAdamSchemaData",
                "GetAdamSchemaClassData",
                "GetAdamSchemaPropertyData",
                "AddSchemaClasstoAdam",
                "AddSchemaAttributetoAdam",
                

             };

            string[] topology = new string[] { 
                "\n:: Topology reporting and management cmdlets ::",
                "GetTopologyData forestName",
                "CreateAdSite newSiteName",
                "CreateAdamSite targetName newSiteName",
                "CreateSubnet newSubnet siteName",
                "CreateSiteLink siteName newLinkName",
                "AddSiteToSiteLink siteName siteLinkName",
                "RemoveSiteFromSiteLink siteName siteLinkName",
                "DeleteAdSite siteName",
                "DeleteAdamSite targetName siteName",
                "DeleteLink linkName",
            };
            string[] replication = new string[] { 
                "\n:: Replication reporting and management cmdlets ::",
                "GetReplicationStateData",
                "ReplicateFromSource partitionDN sourceServer targetServer",
                "ReplicateFromNeighbors targetServer partitionDN",
                "SyncAllServers partitionDN",
                "CreateNewConnection sourceServer targetServer connectionName",
                "SetReplicationConnection server connectionName",
                "DeleteReplicationConnection server connectionName",
            };
            string[] trust = new string[] { 
                "\n:: Trust reporting and management tasks ::",
                "GetCurrentForestTrusts",
                "GetCurrentDomainTrusts",
                "GetTrustWithTargetForest",
                "GetTrustWithTargetDomain",
                "CreateCrossForestTrust targetForest userNameTargetForest password",
                "SetForestTrustAttributes targetForestName",
                "ChangeForestTrustToOutbound targetForestName userNameTargetForest password",
                "AddExcludedDomain targetForestName targetDomainName",
                "DisableDomainNetbiosName targetForestName targetDomainName",
                "RepairTrust targetForestName userNameTargetForest password",
                "RemoveForestTrust targetForestName userNameTargetForest password",
                "RemoveDomainTrust targetDomainName userNameTargetDomain password"

            };

            //string msg = "available commands:";
            //Console.WriteLine(msg);
            //Console.WriteLine("1. Forest, Domain, ADAM\n2. Topology reporting and management\n3. Replication reporting and management \n4. ");
            foreach (string arg in general)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }

            foreach (string arg in topology)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }

            foreach (string arg in replication)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }

            foreach (string arg in schema)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }

            foreach (string arg in trust)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
                String argString = System.String.Concat(arg);
                Console.WriteLine(argString);
                Console.ResetColor();
            }

        }
        */
    }
}