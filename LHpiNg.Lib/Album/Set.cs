namespace LHpiNG.Album
{
    class Set
    {
        //identification
        private string TLA { get; set; }//primary key
        private int MAId { get; set; }//key
        private string Name { get; set; }//key

        //information
        private int CardCount { get; set; }
        private int TokenCount { get; set; }
        private int NontraditionalCount { get; set; }
        private int InsertCount { get; set; }
        private int ReplicaCount { get; set; }
        private bool HasFoil { get; set; }
        private bool HasNonfoil { get; set; }
    }
}