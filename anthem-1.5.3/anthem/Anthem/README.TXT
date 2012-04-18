Release Notes for Version 1.5

See CHANGELOG.TXT for a complete list of changes since version 1.4.

Possible Breaking Changes

1.	Removed support for obsolete OnPreUpdate and BaseHref.

	If you used OnPreUpdate, replace it with OnPreRender and check for Anthem.Manager.IsCallBack=true.
	If you used BaseHref, just remove it.

Other Significant Changes

1.	FileUpload control added to Anthem-2005. Support for <input type="file"> added to both Anthem-2003 and Anthem-2005.
2.	XML Comments added to all controls. This enables Intellisense for the controls in the Anthem library.
2.	Class documentation added to http://anthem-dot-net.sourceforge.net/docs/.
3.	Works with .NET 3.5 installed.
