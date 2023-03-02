using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreInfrastructure.ItemInformation.ItemInformation;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace MechSuitsApi.Classes
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri =  QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
