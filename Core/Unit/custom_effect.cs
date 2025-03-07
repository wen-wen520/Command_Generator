namespace Command_Generator.Core.Unit
{
    internal class custom_effects : base_unit
    {
        // Attributes
        private string user_unit_name = "Default Unit Name";
        private string user_unit_uuid = "Default Unit UUID";
        private string id = "absorption";
        private byte amplifier = 0;
        private int duration = 1;
        private bool ambient = false;
        private bool show_particles = true;
        private bool show_icon = true;

        // Constructor
        public custom_effects(string user_unit_name, string id, byte amplifier, int duration, bool ambient, bool show_particles, bool show_icon)
        {
            this.user_unit_name = user_unit_name;
            this.id = id;
            this.amplifier = amplifier;
            this.duration = duration;
            this.ambient = ambient;
            this.show_particles = show_particles;
            this.show_icon = show_icon;
        }

        // Implementations
        public string toString()
        {
            return "id: " + id + ", amplifier: " + amplifier + ", duration: " + duration + ", ambient: " + ambient + ", show_particles: " + show_particles + ", show_icon: " + show_icon;
        }
        public string toUnit()
        {
            return $"{{{this.toString()}}}";
        }
    }
}
