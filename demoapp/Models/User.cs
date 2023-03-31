﻿using System;
using System.Runtime.Serialization;

namespace demoapp.Models
{
	public class User
	{
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public Address? Address { get; set; }

        public Company? Company { get; set; }


    }

    public class Address
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        public string? Street { get; set; }

        public string? Suite { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public Geo? Geo { get; set; }

    }
    public class Geo
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        public string? Lat { get; set; }

        public string? Ltd { get; set; }

    }
    public class Company
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? CatchPhrase { get; set; }

        public string? Bs { get; set; }

    }
}

