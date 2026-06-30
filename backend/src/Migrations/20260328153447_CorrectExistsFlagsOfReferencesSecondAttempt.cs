using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class CorrectExistsFlagsOfReferencesSecondAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE metabase.method
                SET 
                    ""Reference_Standard_Exists"" = CASE WHEN
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""Reference_Publication_Exists"" = CASE WHEN
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""Reference_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END;

                UPDATE metabase.data_format
                SET
                    ""Reference_Standard_Exists"" = CASE WHEN
                        ""Reference_Standard_Title"" IS NOT NULL OR
                        ""Reference_Standard_Abstract"" IS NOT NULL OR
                        ""Reference_Standard_Section"" IS NOT NULL OR
                        ""Reference_Standard_Year"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""Reference_Standard_Locator"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""Reference_Publication_Exists"" = CASE WHEN
                        ""Reference_Publication_Title"" IS NOT NULL OR
                        ""Reference_Publication_Abstract"" IS NOT NULL OR
                        ""Reference_Publication_Section"" IS NOT NULL OR
                        ""Reference_Publication_Authors"" IS NOT NULL OR
                        ""Reference_Publication_Doi"" IS NOT NULL OR
                        ""Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""Reference_Publication_Urn"" IS NOT NULL OR
                        ""Reference_Publication_WebAddress"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""Reference_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END;

                UPDATE metabase.component
                SET
                    ""PrimeSurface_Reference_Standard_Exists"" = CASE WHEN
                        ""PrimeSurface_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Standard_Locator"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeSurface_Reference_Publication_Exists"" = CASE WHEN
                        ""PrimeSurface_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeSurface_Reference_Publication_WebAddress"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeSurface_Reference_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeSurface_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END,

                    ""PrimeDirection_Reference_Standard_Exists"" = CASE WHEN
                        ""PrimeDirection_Reference_Standard_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Year"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Standard_Locator"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeDirection_Reference_Publication_Exists"" = CASE WHEN
                        ""PrimeDirection_Reference_Publication_Title"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Section"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Authors"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Doi"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_Urn"" IS NOT NULL OR
                        ""PrimeDirection_Reference_Publication_WebAddress"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeDirection_Reference_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""PrimeDirection_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END,

                    ""SwitchableLayers_Reference_Standard_Exists"" = CASE WHEN
                        ""SwitchableLayers_Reference_Standard_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Year"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Prefix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_MainNumber"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Numeration_Suffix"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Standardizers"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Standard_Locator"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""SwitchableLayers_Reference_Publication_Exists"" = CASE WHEN
                        ""SwitchableLayers_Reference_Publication_Title"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Abstract"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Section"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Authors"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Doi"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_ArXiv"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_Urn"" IS NOT NULL OR
                        ""SwitchableLayers_Reference_Publication_WebAddress"" IS NOT NULL
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""SwitchableLayers_Reference_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END,
                    ""SwitchableLayers_Exists"" = CASE WHEN
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
                        THEN TRUE
                      ELSE NULL
                    END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}