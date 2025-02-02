namespace Command_Generator.Core.Unit
{
    internal class enchantment
    {
        //minecraft:enchantments​[upcoming JE 1.21.5]: Contains key-value pairs of levels of enchantments on this item that affect the way the item works.
        //[Int] <enchantment ID>: A single key-value pair, where the key is the resource location of an enchantment, and the value is the level.

        private string user_unit_name = "Default Unit Name";
        private string user_unit_uuid = "Default Unit UUID";
        private string id = "sharpness";
        private byte level = 1;
        private bool show_in_tooltip = true;

        public enchantment(string user_unit_name, string id, byte level, bool show_in_tooltip)
        {
            this.user_unit_name = user_unit_name;
            this.id = id;
            this.level = level;
            this.show_in_tooltip = show_in_tooltip;
        }

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