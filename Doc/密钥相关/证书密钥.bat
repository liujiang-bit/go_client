del 证书密钥.txt
keytool -exportcert -alias oc -keystore oc.keystore | OpenSSL\bin\openssl sha1 -binary | OpenSSL\bin\openssl base64 >> 证书密钥.txt
pause