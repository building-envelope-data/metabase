using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class CorrectExistsFlagsOfReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE metabase.method
                SET 
                    ""Reference_Standard_Exists"" = (
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL
                    ),
                    ""Reference_Publication_Exists"" = (
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""Reference_Exists"" = (
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL OR
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                    );

                UPDATE metabase.data_format
                SET
                    ""Reference_Standard_Exists"" = (
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL
                    ),
                    ""Reference_Publication_Exists"" = (
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""Reference_Exists"" = (
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL OR
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                    );

                UPDATE metabase.component
                SET
                    ""PrimeSurface_Reference_Standard_Exists"" = (
                        ""PrimeSurface_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Locator"" IS NOT NULL
                    ),
                    ""PrimeSurface_Reference_Publication_Exists"" = (
                        ""PrimeSurface_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""PrimeSurface_Reference_Exists"" = (
                        ""PrimeSurface_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Locator"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""PrimeSurface_Exists"" = (
                        ""PrimeSurface_Description"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Locator"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_WebAddress"" IS NOT NULL
                    ),

                    ""PrimeDirection_Reference_Standard_Exists"" = (
                        ""PrimeDirection_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Locator"" IS NOT NULL
                    ),
                    ""PrimeDirection_Reference_Publication_Exists"" = (
                        ""PrimeDirection_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""PrimeDirection_Reference_Exists"" = (
                        ""PrimeDirection_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Locator"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""PrimeDirection_Exists"" = (
                        ""PrimeDirection_Description"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Locator"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_WebAddress"" IS NOT NULL
                    ),

                    ""SwitchableLayers_Reference_Standard_Exists"" = (
                        ""SwitchableLayers_Reference_Standard_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Year"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Locator"" IS NOT NULL
                    ),
                    ""SwitchableLayers_Reference_Publication_Exists"" = (
                        ""SwitchableLayers_Reference_Publication_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Authors"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Doi"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Urn"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""SwitchableLayers_Reference_Exists"" = (
                        ""SwitchableLayers_Reference_Standard_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Year"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Locator"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Authors"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Doi"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Urn"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_WebAddress"" IS NOT NULL
                    ),
                    ""SwitchableLayers_Exists"" = (
                        ""SwitchableLayers_Description"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Year"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Locator"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Authors"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Doi"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Urn"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_WebAddress"" IS NOT NULL
                    );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}