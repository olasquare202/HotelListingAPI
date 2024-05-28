﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        [ForeignKey(nameof(Country))]//Hetel reference d name of class 'Country'
        public int CountryId { get; set; }
        public Country Country { get; set; }//This gives Hotel access to all d fields in class 'Country' i.e Hotel.Country.ShortName e.t.c ShortName of d Country.
       
    } 
}
