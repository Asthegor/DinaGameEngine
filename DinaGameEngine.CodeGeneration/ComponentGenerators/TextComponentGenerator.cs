using DinaGameEngine.Models;

using System.Text;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public class TextComponentGenerator : IComponentGenerator
    {
        public string ComponentType => "Text";

        public void GenerateDraw(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateDrawUser(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateField(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateLoad(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateLoadUser(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateReset(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateResetUser(StringBuilder sb, ComponentModel component, int level)
        {
            
        }

        public void GenerateUpdate(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }

        public void GenerateUpdateUser(StringBuilder sb, ComponentModel component, int level)
        {
            throw new NotImplementedException();
        }
    }
}
