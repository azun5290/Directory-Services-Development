/*=====================================================================
  File:     TrustManagement.cs

  Summary:  Section for cross forest trust management tasks

=====================================================================
                         **                         **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]


namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    public class ADTrustManagement
    {

        public static void CreateCrossForestTrust(string targetForestName, string userNameTargetForest, string password)
        {
            try
            {

                // bind to the current forest
                Forest sourceForest = Forest.GetCurrentForest();


                // get context to the target forest
                DirectoryContext targetContext = new DirectoryContext(
                                                        DirectoryContextType.Forest,
                                                        targetForestName,
                                                        userNameTargetForest,
                                                        password);

                // bind to the target forest
                Forest targetForest = Forest.GetForest(targetContext);

                // create an outbound forest trust
                // The CreateTrustRelationship establishes both sides of the relationship.
                // if you don't have permission to complete the entire relationship, you can
                // use the sourceForest.CreateLocalSideOfTrustRelationship method instead. 

                // Create an bidirectional trust between the source and target forest.
                sourceForest.CreateTrustRelationship(targetForest,
                                                     TrustDirection.Bidirectional);

                Console.WriteLine("\nCross forest trust created.");


                // verify the trust relationship
                sourceForest.VerifyTrustRelationship(targetForest,
                                                     TrustDirection.Bidirectional);

                Console.WriteLine("\nThe forest trust has been successfully validated.");


                // get the trust relationship to report on properties of the trust
                ForestTrustRelationshipInformation forestTrust =
                                    sourceForest.GetTrustRelationship(targetForestName);

                Console.WriteLine("\nNew forest trust: {0} - {1}\n" +
                                "Trust direction: {2}\nTrust type: {3}",
                                forestTrust.SourceName.ToUpper(),
                                forestTrust.TargetName.ToUpper(),
                                forestTrust.TrustDirection,
                                forestTrust.TrustType);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void SetForestTrustAttributes(string targetForestName)
        {
            try
            {

                // bind to the current forest
                Forest sourceForest = Forest.GetCurrentForest();

                // change forest trust attributes
                sourceForest.SetSelectiveAuthenticationStatus(targetForestName, true);
                sourceForest.SetSidFilteringStatus(targetForestName, false);

                Console.WriteLine("\nSelectiveAuthenticationStatus of the " +
                                  "trust is now {0}",
                                  sourceForest.GetSelectiveAuthenticationStatus(
                                                                     targetForestName));

                Console.WriteLine("SidFilteringStatus of the trust is now {0}",
                                  sourceForest.GetSidFilteringStatus(targetForestName));

            }

            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void ChangeForestTrustToOutbound(string targetForestName, string userNameTargetForest, string password)
        {
            try
            {
                // bind to the current forest 
                Forest sourceForest = Forest.GetCurrentForest();

                // get context to the target forest
                DirectoryContext targetContext = new DirectoryContext(
                                                        DirectoryContextType.Forest,
                                                        targetForestName,
                                                        userNameTargetForest,
                                                        password);

                // bind to the target forest
                Forest targetForest = Forest.GetForest(targetContext);

                // update the trust direction
                sourceForest.UpdateTrustRelationship(targetForest,
                                                     TrustDirection.Outbound);

                Console.WriteLine("\nUpdateTrustRelationship succeeded");

                // verify outbound side of the trust relationship. 
                // Pass the name of the target forest. Unlike VerifyTrustRelationship, 
                // there is no need to bind to the target forest or 
                // pass a trust direction. 
                sourceForest.VerifyOutboundTrustRelationship(targetForestName);
                Console.WriteLine("\nVerifyOutboundTrustRelationship succeeded\n");


                // check that the trust direction has been updated
                ForestTrustRelationshipInformation forestTrust =
                    sourceForest.GetTrustRelationship(targetForestName);
                
                //display common trust information
                Console.WriteLine("Forest trust: {0} - {1}\n" +
                                  "Trust direction: {2}\nTrust type: {3}",
                                  forestTrust.SourceName.ToUpper(),
                                  forestTrust.TargetName.ToUpper(),
                                  forestTrust.TrustDirection,
                                  forestTrust.TrustType);

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void AddExcludedDomain(string targetForestName, string targetDomainName)
        {
            try
            {

                // bind to the current forest
                Forest sourceForest = Forest.GetCurrentForest();

                // get the trust relationship
                ForestTrustRelationshipInformation forestTrust =
                    sourceForest.GetTrustRelationship(targetForestName);

                // add a top level name
                forestTrust.ExcludedTopLevelNames.Add(targetDomainName);
                forestTrust.Save();

                Console.WriteLine("\nName suffix routing is now disabled for:");
                foreach (string s in forestTrust.ExcludedTopLevelNames)
                {
                    Console.WriteLine("\t{0}", s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }


        public static void DisableDomainNetBIOSName(string targetForestName, string targetDomainName)
        {
            try
            {

                // bind to the current forest
                Forest sourceForest = Forest.GetCurrentForest();

                //get the trust relationship
                ForestTrustRelationshipInformation forestTrust =
                    sourceForest.GetTrustRelationship(targetForestName);

                foreach (ForestTrustDomainInformation domainInfo in
                    forestTrust.TrustedDomainInformation)
                {
                    if (domainInfo.DnsName == targetDomainName)
                    {
                        domainInfo.Status =
                                ForestTrustDomainStatus.NetBiosNameAdminDisabled;

                        Console.WriteLine("\nNetBIOS Domain Name routing for {0}\n" +
                                                                "is now set to {1}",
                                                                targetDomainName,
                                                                domainInfo.Status);
                    }
                }

                forestTrust.Save();


            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        public static void RepairTrust(string targetForestName, string userNameTargetForest, string password)
        {
            try
            {

                // bind to the current forest 
                Forest sourceForest = Forest.GetCurrentForest();

                // get context to the target forest
                DirectoryContext targetContext = new DirectoryContext(
                                                        DirectoryContextType.Forest,
                                                        targetForestName,
                                                        userNameTargetForest,
                                                        password);

                // bind to the target forest
                Forest targetForest = Forest.GetForest(targetContext);


                // repair the trust when necessary
                sourceForest.RepairTrustRelationship(targetForest);
                Console.WriteLine("\nRepairTrustRelationship succeeded");

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void RemoveForestTrust(string targetForestName, string userNameTargetForest, string password)
        {
            try
            {

                // bind to the current forest 
                Forest sourceForest = Forest.GetCurrentForest();

                // get context to the target forest
                DirectoryContext targetContext = new DirectoryContext(
                                                        DirectoryContextType.Forest,
                                                        targetForestName,
                                                        userNameTargetForest,
                                                        password);

                // bind to the target forest
                Forest targetForest = Forest.GetForest(targetContext);


                // sourceForest.DeleteLocalSideOfTrustRelationship also available for
                // removing one side of a trust relationship

                // delete the forest trust
                sourceForest.DeleteTrustRelationship(targetForest);
                Console.WriteLine("\nDeleteTrustRelationship succeeded");
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        public static void RemoveDomainTrust(string targetDomainName, string userNameTargetDomain, string password)
        {
            try
            {

                // bind to the current domain
                Domain sourceDomain = Domain.GetComputerDomain();

                // get context to the target domain
                DirectoryContext context = new DirectoryContext(
                                                            DirectoryContextType.Domain,
                                                            targetDomainName,
                                                            userNameTargetDomain,
                                                            password);

                // bind to the target domain
                Domain targetDomain = Domain.GetDomain(context);


                // sourceDomain.DeleteLocalSideOfTrustRelationship(targetDomainName) also available
                // to remove one side of the domain trust relationship.
                // delete the domain trust
                sourceDomain.DeleteTrustRelationship(targetDomain);
                Console.WriteLine("\nDeleteTrustRelationship succeeded");
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

    }
}
