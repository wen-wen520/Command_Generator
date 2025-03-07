namespace Command_Generator.Core.Unit
{
    internal class enchantment : base_unit
    {
        // Attributes
        private string user_unit_name = "Default Unit Name";
        private string user_unit_uuid = "Default Unit UUID";
        private string id = "sharpness";
        private byte level = 1;
        private bool show_in_tooltip = true;

        // Constructor
        public enchantment(string user_unit_name, string id, byte level, bool show_in_tooltip)
        {
            this.user_unit_name = user_unit_name;
            this.id = id;
            this.level = level;
            this.show_in_tooltip = show_in_tooltip;
        }

        // Implementations
        public string toString()
        {
            return $"{{\"{id}\":{level}}}";
        }
        public string toUnit()
        {
            return $"{{\"{id}\":{level}}}";
        }
    }
}