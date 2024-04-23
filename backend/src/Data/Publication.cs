using System;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data
{
    [Owned]
    public sealed class Publication
        : IReference
    {
        [MinLength(1)] public string? Title { get; private set; }

        [MinLength(1)] public string? Abstract { get; private set; }

        [MinLength(1)]
        [GraphQLDescription("Referenced section")]
        public string? Section { get; private set; }

        [MinLength(1)] public string[]? Authors { get; private set; }

        [MinLength(1)]
        [RegularExpression("^(10[.][0-z/.]*)$")]
        [GraphQLDescription(
            "The Digital Object Identifier (DOI) is a very important persistent identifier for publications. It MUST be defined here if it is available for a publication.")]
        public string? Doi { get; private set; }

        [MinLength(1)]
        [RegularExpression("^(arXiv:)[0-z./]*$")]
        [GraphQLDescription(
            "The website arXiv.org is a free and open-access archive for publications. The arXiv identifier can be used to define a publication.")]
        public string? ArXiv { get; private set; }

        [MinLength(1)]
        [RegularExpression("^(urn:)[0-z:./-]*$")]
        [GraphQLDescription(
            "A Uniform Resource Name (URN) can be used to define a publication. TODO: Improve the regex pattern to further restrict the string.")]
        public string? Urn { get; private set; }

        [Url]
        [GraphQLDescription(
            "If a persistent identifiert like DOI is defined above, this webAdress can define a convenient web address to access the publication. However, if no persistent identifier exist, this web address is the only identifier of this publication. In this case, it is important to choose a web address with a high probability to persist long.")]
        public Uri? WebAddress { get; private set; }

        public Publication(
            string? title,
            string? @abstract,
            string? section,
            string[]? authors,
            string? doi,
            string? arXiv,
            string? urn,
            Uri? webAddress
        )
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Authors = authors;
            Doi = doi;
            ArXiv = arXiv;
            Urn = urn;
            WebAddress = webAddress;
        }
    }
}