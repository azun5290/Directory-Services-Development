/*=====================================================================
  File:     ReplicationData.cs

  Summary:  Demonstrates replication related classes and methods
=====================================================================
                        ** SAMPLE CODE **
 */

using System;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

using System.Text; //Added for the BuildFilterOctetString method

[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: DirectoryServicesPermission(SecurityAction.RequestMinimum)]

namespace MSDN.Samples.DirectoryServices.ActiveDirectory
{
    class ReplicationState
    {

        public static void GetData()
        {
            try
            {

                // get a domain context
                DirectoryContext context = new DirectoryContext(
                                                    DirectoryContextType.Domain);

                // alternative for testing a specific server's replication status.
                // this uses the GetDomainController context remarked below.
                // DirectoryContext context = new DirectoryContext(
                //                               DirectoryContextType.DirectoryServer, 
                //                               "sea-dc-02.fabrikam.com");

                // bind to a domain controller in the domain. The context for this FindOne
                // method must be domain
                DomainController dc = DomainController.FindOne(context);

                // alternative for getting a DC to check its replication status.
                // DomainController dc = DomainController.GetDomainController(context);

                // retrieve replication cursor information for each replicated partition
                Console.WriteLine("\nReplication cursor data for each partition\n");
                foreach (string partition in dc.Partitions)
                {
                    Console.WriteLine("\tPartition {0}", partition);
                    foreach (ReplicationCursor cursor in dc.GetReplicationCursors(partition))
                    {
                        Console.WriteLine("\t\tSourceServer: {0}\n" +
                                          "\t\tLastSuccessfulSyncTime: {1}\n" +
                                          "\t\tSourceInvocationId: {2}\n" +
                                          "\t\t octet string format: {3}\n" +
                                          "\t\tusn: {4}\n",
                                          cursor.SourceServer,
                                          cursor.LastSuccessfulSyncTime,
                                          cursor.SourceInvocationId,
                                          BuildFilterOctetString(cursor.SourceInvocationId.ToByteArray()),
                                          cursor.UpToDatenessUsn
                                          );
                    }
                }

                // retrieve replication neighbor information
                Console.WriteLine("\nReplication neighbor data\n");
                foreach (string partition in dc.Partitions)
                {
                    Console.WriteLine("\tPartition: {0}", partition);
                    foreach (ReplicationNeighbor neighbor in
                                                      dc.GetReplicationNeighbors(partition))
                    {
                        Console.WriteLine("\t\tSourceServer: {0}\n" +
                                          "\t\tReplicationNeighborFlag: {1}\n" +
                                          "\t\tUsnAttributeFilter: {2}\n" +
                                          "\t\tLastSuccessfulSync: {3}\n",
                                          neighbor.SourceServer,
                                          neighbor.ReplicationNeighborOption,
                                          neighbor.UsnAttributeFilter,
                                          neighbor.LastSuccessfulSync);
                    }
                }

                // retrieve the inbound replication connections
                // other servers initiate replication to this server
                Console.WriteLine("\nInbound replication connection data\n");

                foreach (ReplicationConnection con in dc.InboundConnections)
                {

                    Console.WriteLine("\tReplication connection name (cn): {0}\n " +
                                      "\tSourceServer: {1}\n " +
                                      "\tDestinationServer: {2}\n" +
                                      "\tTransport type: {3}\n" +
                                      "\tConnection schedule owned by user: {4}\n",
                                      con.Name,
                                      con.SourceServer,
                                      con.DestinationServer,
                                      con.TransportType,
                                      con.ReplicationScheduleOwnedByUser
                                      );
                }

                // retrieve the outbound replication connections
                // this server initiates replication to the following servers
                Console.WriteLine("\nOutbound replication connection data\n");

                foreach (ReplicationConnection con in dc.OutboundConnections)
                {

                    Console.WriteLine("\tReplication connection (cn): {0}\n" +
                                      "\tSourceServer: {1}\n" +
                                      "\tDestinationServer: {2}\n" +
                                      "\tTransport type: {3}\n",
                                      con.Name,
                                      con.SourceServer,
                                      con.DestinationServer,
                                      con.TransportType
                                      );
                }

                // A simple approach using S.DS to get to the dn of the current domain
                DirectoryEntry de = new DirectoryEntry();
                string targetDomainDN = de.Properties["distinguishedName"].Value.ToString();

                // UPDATE THIS VALUE WITH A VALID RDN OF AN OBJECT IN YOUR DOMAIN. 
                // If you want to evaluate the replication status of a child object 
                // of an object that is a child of 
                // the domain, then include it's path up to the domain level:
                // e.g., "cn=user1,ou=techwriters"
                string AdObjectRdn = "ou=techwriters";

                string objectPath = AdObjectRdn + "," + targetDomainDN;

                // retrieve the replication metadata information for a specific object
                Console.WriteLine("\nReplication metadata data for object path {0}\n", objectPath);

                ActiveDirectoryReplicationMetadata metadata = null;

                try
                {
                    metadata = dc.GetReplicationMetadata(objectPath);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("{0}.\nPlease specify an existing " +
                                       "object for the ADObjectRdn variable",
                                       e.Message);
                    return;
                }

                // iterate through the attributes associated with this object
                foreach (string attribute in metadata.AttributeNames)
                {
                    Console.WriteLine("\tAttribute lDAPDisplayName: {0}\n", attribute);

                    // for each attribute, assign an AttributeMetadata object by 
                    // passing the ActiveDirectoryReplicationMetadata object the 
                    // lDapDisplayName of the attribute to inspect. The 
                    // ActiveDirectoryReplicationMetadata object gets the attribute
                    // to inspect.
                    AttributeMetadata replicationData = metadata[attribute];

                    Console.WriteLine("\t\tOriginatingServer: {0}\n" +
                                      "\t\tOriginatingChangeUsn: {1}\n" +
                                      "\t\tLocalChangeUsn: {2}\n" +
                                      "\t\tLastOriginatingChangeTime: {3}\n",
                                      replicationData.OriginatingServer,
                                      replicationData.OriginatingChangeUsn,
                                      replicationData.LocalChangeUsn,
                                      replicationData.LastOriginatingChangeTime);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\n{0}:{1}",
                                  e.GetType().Name, e.Message);
            }

        }

        // convert a hex value to an octet string
        // to compare the hex value returned by the InvocationID properties
        // with the value as it appears in ADSI Edit
        // This is derived from Listing 4.2 in The .NET Developer's Guide
        // to Directory Services Programming by Joe Kaplan and Ryan Dunn.
        static string BuildFilterOctetString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("0x{0} ", bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }

}
