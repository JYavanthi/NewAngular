using System;
using System.Collections.Generic;

namespace sanchar6tBackEnd.Models;

public partial class CityPair
{
    public int CityPairId { get; set; }

    public int OriginId { get; set; }

    public int DestinationId { get; set; }

    public DateTime TravelDate { get; set; }

    public int? SearchAcceptedStaleness { get; set; }

    public int? BookAcceptedStaleness { get; set; }

    public DateTime LastSyncedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDt { get; set; }
}
