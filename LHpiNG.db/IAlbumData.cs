using LHpiNG.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LHpiNG.db
{
    public interface  IAlbumData
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        //DbSet<LHpiNG.Album.Set> Sets { get; set; }
        //DbSet<LHpiNG.Album.CardInfo> CardInfos { get; set; }
        //DbSet<LHpiNG.Album.InventoryCard> InventoryCard { get; set; }

    }
}