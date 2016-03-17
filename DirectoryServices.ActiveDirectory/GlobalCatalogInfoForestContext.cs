/*=====================================================================
  File:     GlobalCatalogInfoForextContext.cs

  Summary:  Section for finding and returning information about a global 
            catalog server in an Active Directory forest.

---------------------------------------------------------------------
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
    public partial class FromForest
    {

        public static void GetGlobalCatalogConfigData()
        {
            Console.WriteLine("<--GLOBAL CATALOG CONFIGURATION DATA-->\n");

            // Get a forest context
            DirectoryContext forestContext = new DirectoryContext(
                                                DirectoryContextType.Forest);
            
            GlobalCatalog gc;

            try
            {
                // bind to a global catalog server in the forest
                gc = GlobalCatalog.FindOne(forestContext);
                Console.WriteLine("Finding one global catalog " +
                                                    "in the current forest:");
                Console.WriteLine("Name: {0}", gc);
                Console.WriteLine("Site: {0}", gc.SiteName);

                // list roles held by the GC
                Console.WriteLine("\nRoles:");
                foreach (ActiveDirectoryRole role in gc.Roles)
                {
                    Console.WriteLine("\t{0}", role);
                }

                // list partitions hosted by the GC
                Console.WriteLine("\nPartitions hosted by this global catalog:");
                foreach (string partition in gc.Partitions)
                {
                    Console.WriteLine("\t{0}", partition);
                }
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // gc not found
                Console.WriteLine(e.Message);
            }

        }
    }
}