/*=====================================================================
  File:     TrustData.cs

  Summary: Demonstrates retrieving trust data from AD using the
           System.DirectoryServices.ActiveDirectory classes
           
=====================================================================
                         ** SAMPLE CODE **
*/

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]


namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{

    public class ADTrust
    {

        public static void GetCurrentForestTrusts()
        {
            try
            {

                // get the current forest - get context and bind to the forest
                Forest currentForest = Forest.GetCurrentForest();

                // Retrieve all the forest trusts
                Console.WriteLine("\nRetrieve all the forest trusts " +
                                  "with current forest:\n");

                foreach (ForestTrustRelationshipInformation forestTrust in
                                               currentForest.GetAllTrustRelationships())
                {
                    // for each forest trust relationship, get its properties
                    Console.WriteLine("\nForest trust: {0} - {1}\n" +
                                    "Trust direction: {2}\nTrust type: {3}",
                                    forestTrust.SourceName.ToUpper(),
                                    forestTrust.TargetName.ToUpper(),
                                    forestTrust.TrustDirection,
                                    forestTrust.TrustType);

                    // display selective authentication status of a forest trust
                    Console.WriteLine("SelectiveAuthenticationStatus of the trust: {0}",
                        currentForest.GetSelectiveAuthenticationStatus(
                                                    forestTrust.TargetName));

                    // display Sid filtering status of a forest trust
                    Console.WriteLine("SidFilteringStatus of the trust: {0}",
                        currentForest.GetSidFilteringStatus(
                                                    forestTrust.TargetName));

                    // list the top level domain names of the trust
                    Console.WriteLine("\nTopLevelNames:");
                    foreach (TopLevelName top in forestTrust.TopLevelNames)
                    {
                        Console.WriteLine("\t{0}, status: {1}", top.Name, top.Status);
                    }
                    
                    // list excluded top level domain names of the trust
                    // this is referred to in Active Directory as name suffix exceptions
                    Console.WriteLine("\nExcludedTopLevelNames:");
                    foreach (string excluded in forestTrust.ExcludedTopLevelNames)
                    {
                        Console.WriteLine("\t{0}\n", excluded);
                    }
                    
                    // get domain-specific information about the forest trust
                    Console.WriteLine("\nForest Trust Domain Information:");
                    foreach (ForestTrustDomainInformation domainInfo in
                                                       forestTrust.TrustedDomainInformation)
                    {
                        Console.WriteLine("\n\tDNS name: {0}\n\tNetBIOS name: {1}\n" +
                                          "\tdomain sid: {2}\n\tstatus: {3}",
                                          domainInfo.DnsName,
                                          domainInfo.NetBiosName,
                                          domainInfo.DomainSid,
                                          domainInfo.Status
                                          );
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void GetTrustWithTargetForest(string targetForestName)
        {
            try
            {
                //get the current forest context and bind to the forest
                Forest currentForest = Forest.GetCurrentForest();

                Console.WriteLine("\nTrust with the target forest:\n");

                ForestTrustRelationshipInformation forestTrust =
                                   currentForest.GetTrustRelationship(targetForestName);

                //display common trust information
                Console.WriteLine("Forest trust: {0} - {1}\n" +
                                  "Trust direction: {2}\nTrust type: {3}",
                                  forestTrust.SourceName.ToUpper(),
                                  forestTrust.TargetName.ToUpper(),
                                  forestTrust.TrustDirection,
                                  forestTrust.TrustType);

                //display selective authentication status of the forest trust
                Console.WriteLine("SelectiveAuthenticationStatus of the trust: {0}",
                    currentForest.GetSelectiveAuthenticationStatus(targetForestName));

                //display Sid filtering status of the domain trust
                Console.WriteLine("SidFilteringStatus of the trust: {0}",
                    currentForest.GetSidFilteringStatus(targetForestName));

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }


        //Demonstrates how to retrieve all trust relationships with the current domain
        public static void GetCurrentDomainTrusts()
        {
            try
            {

                // get a domain context and bind to the current domain
                Domain currentDomain = Domain.GetCurrentDomain();


                // Retrieve all the domain trusts
                Console.WriteLine("\nRetrieve all domain trusts with the current domain:\n");
                foreach (TrustRelationshipInformation domainTrust in
                                               currentDomain.GetAllTrustRelationships())
                {
                    // for each domain trust relationship, get its properties
                    Console.WriteLine("\nDomain trust: {0} - {1}" +
                                      "\ntrust direction: {2}" +
                                      "\ntrust type: {3}",
                                      domainTrust.SourceName.ToUpper(),
                                      domainTrust.TargetName.ToUpper(),
                                      domainTrust.TrustDirection,
                                      domainTrust.TrustType);

                    //display selective authentication status of the domain trust
                    Console.WriteLine("SelectiveAuthenticationStatus of the trust: {0}",
                        currentDomain.GetSelectiveAuthenticationStatus(domainTrust.TargetName));

                    //display Sid filtering status of the domain trust
                    Console.WriteLine("SidFilteringStatus of the trust: {0}",
                        currentDomain.GetSidFilteringStatus(domainTrust.TargetName));

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

        public static void GetTrustWithTargetDomain(string targetDomainName)
        {
            try
            {

                // get the current domain
                Domain currentDomain = Domain.GetCurrentDomain();

                // Retrieve trust by name
                Console.WriteLine("\nRetrieve the trust with the target domain:\n");

                TrustRelationshipInformation domainTrust =
                                   currentDomain.GetTrustRelationship(targetDomainName);

                Console.WriteLine("Domain trust: {0} - {1}" +
                                  "\ntrust direction: {2}" +
                                  "\ntrust type: {3}",
                                  domainTrust.SourceName,
                                  domainTrust.TargetName,
                                  domainTrust.TrustDirection,
                                  domainTrust.TrustType);

                // display selective authentication status of the domain trust
                Console.WriteLine("SelectiveAuthenticationStatus of the trust: {0}",
                    currentDomain.GetSelectiveAuthenticationStatus(targetDomainName));

                // display Sid filtering status of the domain trust
                Console.WriteLine("SidFilteringStatus of the trust: {0}",
                    currentDomain.GetSidFilteringStatus(targetDomainName));

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n\t{0}\n{1}",
                                  e.GetType().Name, e.Message);
            }
        }

    }

}
