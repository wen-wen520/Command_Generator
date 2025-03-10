namespace Command_Generator.Core.Unit
{
    internal class enchantment : base_unit
    {
        // Attributes
        private string user_unit_name { get; set; } = "Default Unit Name";
        private string user_unit_uuid { get; set; } = "Default Unit UUID";
        private string id { get; set; } = "sharpness";
        private byte level { get; set; } = 1;
        private bool show_in_tooltip { get; set; } = true;

        // Constructor
        public enchantment(string user_unit_name, string id, byte level, bool show_in_tooltip)
        {
            this.user_unit_name = user_unit_name;
            this.user_unit_uuid = System.Guid.NewGuid().ToString();
            this.id = id;
            this.level = level;
            this.show_in_tooltip = show_in_tooltip;
        }

        // Implementations
        public string toString(string action = null)
        {
            if (action == "save") { return Newtonsoft.Json.JsonConvert.SerializeObject(this); }
            return $"\"{id}\":{level}";
        }
        public string toUnit()
        {
            return $"{{{this.toString()}}}";
        }
        public async void saveUnit()
        {
            await JsonFileIO.WriteTextToFileAsync($"{user_unit_uuid}.json", this.toString("save"));
        }
    }
}