<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="UTF-8"/>
  <xsl:template match="/">
    <html>
      <body>
        <ul>
          <xsl:for-each select="//book">
            <li>
              <xsl:value-of select="title"/>
              <xsl:call-template name="render-authors">
                <xsl:with-param name="authors" select="authors/author"/>
              </xsl:call-template>
            </li>
          </xsl:for-each>
        </ul>
      </body>
    </html>
  </xsl:template>
  <xsl:template name="render-authors">
    <xsl:param name="authors"/>
    <ul>
    <xsl:for-each select="$authors">
      <li><xsl:value-of select="."/></li>
    </xsl:for-each>
    </ul>
  </xsl:template>
</xsl:stylesheet>
