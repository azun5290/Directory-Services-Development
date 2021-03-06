/*=====================================================================
  File:     ExtendSchema.cs

  Summary:  Section for extending an ADAM instance schema and an AD 
  schema by adding an attribute and class schema object.
---------------------------------------------------------------------
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
    public class ExtendSchema
    {
        // This connection string is to an ADAM instance running on a DC
        // using the default port assigned on a DC to the first ADAM instance
        static string connectionString = "localhost:50000";

        // define an lDAPDisplayName
        // used in both the CreateNewAttribute and CreateNewClass methods
        static string newAttributeLdapDisplayName = "newAttribute1";

        // establish an ADAM context
        static DirectoryContext adamContext = new
            DirectoryContext(DirectoryContextType.DirectoryServer, connectionString);

        // You can use the DirectoryServer or Forest value of the DirectoryContextType enumeration 
        // for connecting to and then extending an AD schema.
        // However, use caution when extended an AD schema. Additions are not easily reversible


        public static void CreateNewAttribute()
        {

            //specify a common name
            string newAttributeCommonName = "New-Attribute1";

            //specify an OID value. The root name was generated by oidgen.exe
            string newAttributeOid =
                "1.2.840.113556.1.4.7000.233.28688.28684.8.145234.1728134.2034934.1932637.1";

            // specify a syntax
            ActiveDirectorySyntax syntax = ActiveDirectorySyntax.CaseIgnoreString;

            // create a new attribute schema object
            ActiveDirectorySchemaProperty newAttribute =
                                                new ActiveDirectorySchemaProperty(
                                                        adamContext,
                                                        newAttributeLdapDisplayName);

            // set attributes for this schema attribute object
            newAttribute.CommonName = newAttributeCommonName;
            newAttribute.Oid = newAttributeOid;
            newAttribute.IsSingleValued = true;
            newAttribute.Syntax = syntax;

            // do not replicate to the global catalog
            // the default for isMemberOfPartialAttributeSet is already False, but this 
            // setting is irrelevant for an ADAM instance.
            // newAttribute.IsInGlobalCatalog = false;

            try
            {
                // save the new attribute schema object to the schema
                newAttribute.Save();
            }
            catch (ActiveDirectoryObjectExistsException e)
            {
                // an object by this name already exists in the schema
                Console.WriteLine("The schema object \"{0}\" was not created. {1}",
                    newAttributeLdapDisplayName, e.Message);
                return;
            }

            catch (ActiveDirectoryOperationException e)
            {
                // a call to the underlying directory was rejected
                Console.WriteLine("The schema object \"{0}\" was not created. {0}",
                            newAttributeLdapDisplayName, e.Message);
                return;

            }

            Console.WriteLine("Attribute schema object \"{0}\" created successfully.",
                                                    newAttributeLdapDisplayName);
        }

        public static void CreateNewClass()
        {
            // specify a common name 
            string newClassCommonName = "new-Class";

            // specify an lDAPDisplayName 
            string newClassLdapDisplayName = "newClass";

            // specify an OID value. The root name was generated by oidgen.exe
            string newClassOid =
                "1.2.840.113556.1.5.7000.111.28688.28684.8.240397.1734810.1181742.544876.1";

            string subClassOf = "top";

            string possibleSuperior = "organizationalUnit";

            // add an optional attribute to the new schema class object 
            // This example adds the new attribute created in the CreateNewAttribute method
            string newClassOptionalAttribute = newAttributeLdapDisplayName;


            // create a new class object
            ActiveDirectorySchemaClass newClass =
                                            new ActiveDirectorySchemaClass(
                                                adamContext,
                                                newClassLdapDisplayName);

            // set the attribute values for this schema class object
            newClass.CommonName = newClassCommonName;
            newClass.Oid = newClassOid;

            newClass.Type = SchemaClassType.Structural;

            // assign the parent class
            newClass.SubClassOf = ActiveDirectorySchemaClass.FindByName(adamContext,
                                                            subClassOf);

            // add the previously created attribute as an optional attribute
            newClass.OptionalProperties.Add(
                ActiveDirectorySchemaProperty.FindByName(adamContext,
                                              newClassOptionalAttribute));

            //add an OU as a possible superior so that this class can be 
            //instantiated in an OU
            newClass.PossibleSuperiors.Add(
                ActiveDirectorySchemaClass.FindByName(adamContext,
                                            possibleSuperior));

            // save the new class to the schema
            try
            {
                newClass.Save();
            }

            catch (ActiveDirectoryObjectExistsException e)
            {
                // an schema object by this name already exists in the schema
                Console.WriteLine("The schema object {0} was not created. {0}",
                                            newClassLdapDisplayName, e.Message);
                return;
            }

            catch (ActiveDirectoryOperationException e)
            {
                // a call to the underlying directory was rejected
                Console.WriteLine("The schema object {0} was not created. {0}",
                            newClassLdapDisplayName, e.Message);
                return;

            }

            Console.WriteLine("Class \"{0}\" created successfully.",
                                                  newClassLdapDisplayName);
        }

    }
}
