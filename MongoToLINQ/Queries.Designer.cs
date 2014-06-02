﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeSharp.MongoToLINQ {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Queries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Queries() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CodeSharp.MongoToLINQ.Queries", typeof(Queries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Asset : { Rules : { &quot;$elemMatch&quot; : { Id : { $gte : 19 } } } } }.
        /// </summary>
        internal static string ElemMatch {
            get {
                return ResourceManager.GetString("ElemMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Age : { $gt :30, $lt : 100 }}.
        /// </summary>
        internal static string GreaterThan {
            get {
                return ResourceManager.GetString("GreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Asset : { Id : { $in : [1,2,3,4,5] } } }.
        /// </summary>
        internal static string In {
            get {
                return ResourceManager.GetString("In", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Asset : { $or : [ { id :{$gt : 30}}, {id :{$lt : 5}}] } }.
        /// </summary>
        internal static string InnerProperty {
            get {
                return ResourceManager.GetString("InnerProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { $not : { Age : 20 } }.
        /// </summary>
        internal static string Not {
            get {
                return ResourceManager.GetString("Not", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Asset : null }.
        /// </summary>
        internal static string Null {
            get {
                return ResourceManager.GetString("Null", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { $or: [ { Status: &quot;A&quot; } ,
        ///             { Age: 50 } ] }.
        /// </summary>
        internal static string Or {
            get {
                return ResourceManager.GetString("Or", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Age : 20 }.
        /// </summary>
        internal static string Simple {
            get {
                return ResourceManager.GetString("Simple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { Asset : { Rules : { $size : { $gt : 0 } } } }.
        /// </summary>
        internal static string Size {
            get {
                return ResourceManager.GetString("Size", resourceCulture);
            }
        }
    }
}
