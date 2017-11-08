﻿using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebSwaggerAAD
{
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // Determine if the operation has the Authorize attribute
            var authorizeAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<AuthorizeAttribute>();
            if (!authorizeAttributes.Any())
                return;
            // Initialize the operation.security property
            if (operation.security == null)
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();
            // Add the appropriate security definition to the operation
            var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            {
                { "oauth2", Enumerable.Empty<string>() }
            };
            operation.security.Add(oAuthRequirements);
        }
    }
}