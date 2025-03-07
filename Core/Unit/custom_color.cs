namespace Command_Generator.Core.Unit
{
    internal class custom_color : base_unit
    {

        // Implementations
        public string toString()
        {
            return "color:";
        }
        public string toUnit()
        {
            return "\"color\":\"{color}\"";
        }

    }
}
