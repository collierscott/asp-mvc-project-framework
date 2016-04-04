using Project.Domain.Models.Entities;

namespace Project.WebUI.Models {

    public class ToolHovermessageModel {

        public string Module { get; set; }
        public Tool Tool { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public int ChamberHeight { get; set; }
        public int PortHeight { get; set; }


        public int HovermessageChamberHeight { get; set; }
        public int HovermessageChamberWidth { get; set; }
        //public int HovermessagePortHeight { get; set; }
        public int HovermessagePortWidth { get; set; }

        public ToolHovermessageModel() {
            Height = 15;
            Width = 45;

            ChamberHeight = 15;
            PortHeight = 4;

            HovermessageChamberHeight = 45;
            HovermessageChamberWidth = 100;
            //HovermessagePortHeight = 45;
            HovermessagePortWidth = 45;
        }
    }

}