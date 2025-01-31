﻿using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string name)
        {
            Regex regex = new Regex(@"%(.*?)%"); // use var here
            string connString = _configuration.GetConnectionString(name);

            MatchCollection mc = regex.Matches(connString);
            if (mc.Count != 0) // better to invert if/else
            {
                foreach (Match m in mc) // give good names
                {
                    var envVal = Environment.GetEnvironmentVariable(m.Value.Replace("%", ""));
                    if(envVal != null)
                        connString = connString.Replace(m.Value, envVal);
                }

                return connString;
            }
            else
                return string.Empty;
        }
    }
}
