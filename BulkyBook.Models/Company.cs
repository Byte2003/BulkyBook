using MessagePack;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
	public class Company
	{
		
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		public string? StreetAddress { set; get; }

		public string? City { set; get; }

		public string? State { get; set; }

		public string? PostalCode { get; set; }

		public string? PhoneNumber { get; set; }

	}
}
