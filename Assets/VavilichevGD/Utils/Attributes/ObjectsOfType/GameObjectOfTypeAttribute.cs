using System;

namespace VavilichevGD.Utils.Attributes.ObjectsOfType {
    public class GameObjectOfTypeAttribute : ObjectOfTypeAttributeBase {
        public GameObjectOfTypeAttribute(Type type, bool allowSceneObjects = true) : base(type, allowSceneObjects) {
        }
    }
}