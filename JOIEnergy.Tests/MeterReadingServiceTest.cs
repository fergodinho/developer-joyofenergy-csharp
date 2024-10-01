using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private MeterReadingService meterReadingService;

        public MeterReadingServiceTest()
        {
            meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new ElectricityReading() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            });
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
            });

            var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Equal(3, electricityReadings.Count);
        }

        [Fact]
        public void GivenLastSevenDaysReadings()
        {
			var SmartMeterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());
            
            DateTime Now = DateTime.Now;

			SmartMeterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
				new ElectricityReading() { Time = Now            , Reading = 25m },
                new ElectricityReading() { Time = Now.AddDays(1) , Reading = 50m },
                new ElectricityReading() { Time = Now.AddDays(-7), Reading = 30m },
                new ElectricityReading() { Time = Now.AddDays(-8), Reading = 40m },
			});

            var readings  = SmartMeterReadingService.GetElectricityReadingsLastSevenDays(SMART_METER_ID);

            Assert.True(readings.Count == 3);
		}

    }
}
