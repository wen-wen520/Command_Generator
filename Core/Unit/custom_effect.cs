namespace Command_Generator.Core.Unit
{
    internal class custom_effect : base_unit
    {
        // Attributes
        public string user_unit_name { get; set; } = "Default Unit Name";
        public string user_unit_uuid { get; set; } = "Default Unit UUID";
        public string id { get; set; } = "absorption";
        public byte amplifier { get; set; } = 0;
        public int duration { get; set; } = 1;
        public bool ambient { get; set; } = false;
        public bool show_particles { get; set; } = true;
        public bool show_icon { get; set; } = true;

        // Constructor
        public custom_effect(string file_name) {
            custom_effect temp = JsonFileIO.ReadJsonFromFileAsync<custom_effect>(file_name).Result;
            this.user_unit_name = temp.user_unit_name;
            this.user_unit_uuid = temp.user_unit_uuid;
            this.id = temp.id;
            this.amplifier = temp.amplifier;
            this.duration = temp.duration;
            this.ambient = temp.ambient;
            this.show_particles = temp.show_particles;
            this.show_icon = temp.show_icon;
        }
        public custom_effect(string user_unit_name, string id, byte amplifier, int duration, bool ambient, bool show_particles, bool show_icon)
        {
            this.user_unit_name = user_unit_name;
            this.user_unit_uuid = System.Guid.NewGuid().ToString();
            this.id = id;
            this.amplifier = amplifier;
            this.duration = duration;
            this.ambient = ambient;
            this.show_particles = show_particles;
            this.show_icon = show_icon;
        }

        // Implementations
        public string toString(string action = null)
        {
            if (action == "save"){ return Newtonsoft.Json.JsonConvert.SerializeObject(this); }

            return $"{{\"id\":\"{id}\",\"amplifier\":{amplifier},\"duration\":{duration},\"ambient\":{ambient},\"show_particles\":{show_particles},\"show_icon\":{show_icon}}}";
        }

        public bool equals(custom_effect obj)
        {
            if (obj == null) { return false; }
            return this.id == obj.id && this.amplifier == obj.amplifier && this.duration == obj.duration && this.ambient == obj.ambient && this.show_particles == obj.show_particles && this.show_icon == obj.show_icon;
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
