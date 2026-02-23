using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sanchar6tBackEnd.Repositories
{
    public class citiesRepository : Icities
    {
        private readonly Sanchar6tDbContext _context;

        public citiesRepository(Sanchar6tDbContext context)
        {
            _context = context;
        }

        // ✅ SAVE / UPDATE CITY (NO StateId here)
        public async Task<CommonRsult> SaveCitiesAsync(List<Ecities> cities)
        {
            var result = new CommonRsult { Success = false, Message = "Failed to save cities" };

            if (cities == null || cities.Count == 0)
                return result;

            try
            {
                foreach (var city in cities)
                {
                    var existingCity = await _context.Cities.FindAsync(city.Id);

                    if (existingCity == null)
                    {
                        var newCity = new City
                        {
                            Id = city.Id,
                            Name = city.Name,
                            CreatedBy = city.CreatedBy,
                            CreatedDt = DateTime.Now
                        };

                        await _context.Cities.AddAsync(newCity);
                    }
                    else
                    {
                        existingCity.Name = city.Name;
                        existingCity.ModifiedBy = city.ModifiedBy;
                        existingCity.ModifiedDt = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Cities saved successfully";
            }
            catch (Exception ex)
            {
                result.Message = $"Error saving cities: {ex.Message}";
            }

            return result;
        }

        // ✅ GET CITIES BY STATE ID (USED FOR DROPDOWN)
        public async Task<CommonRsult> GetCitiesByStateIdAsync(int stateId)
        {
            var result = new CommonRsult { Success = false, Message = "No cities found" };

            try
            {
                var data = await _context.Cities
                    .Where(c => c.StateId == stateId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                result.Success = true;
                result.Data = data;
                result.Message = "Cities retrieved successfully";
            }
            catch (Exception ex)
            {
                result.Message = $"Error fetching cities: {ex.Message}";
            }

            return result;
        }

        // ✅ GET ALL CITIES (OPTIONAL)
        public async Task<CommonRsult> GetCitiesAsync()
        {
            var result = new CommonRsult { Success = false, Message = "No cities found" };

            try
            {
                var data = await _context.Cities
                    .Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                    .OrderBy(c => c.Name)
                    .ToListAsync();

                result.Success = true;
                result.Data = data;
                result.Message = "Cities retrieved successfully";
            }
            catch (Exception ex)
            {
                result.Message = $"Error fetching cities: {ex.Message}";
            }

            return result;
        }
    }
}
