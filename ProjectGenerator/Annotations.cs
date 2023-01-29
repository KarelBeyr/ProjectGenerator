using System.ComponentModel.DataAnnotations;

namespace ProjectGenerator.Annotations;

public class NotInDbAttribute : System.Attribute { }
public class OnlyInDbAttribute : System.Attribute { }

public class DbEntityAttribute : System.Attribute { }
public class ModelAttribute : System.Attribute { }
public class OnlyCreateAttribute : System.Attribute { }
public class PrimaryKeyAttribute : System.Attribute { }
public class CommentSummaryAttribute : System.Attribute 
{
    public string Text { get; set; }
    public CommentSummaryAttribute (string text)
    {
        Text = text;
    }
}
