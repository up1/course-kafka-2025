#!/bin/bash

set -o nounset \
    -o errexit

printf "Deleting previous (if any)..."
rm -rf secrets
mkdir secrets
mkdir -p tmp
echo " OK!"
# Generate CA key
printf "Creating CA..."
openssl req -new -x509 -keyout secrets/datahub-ca.key -out secrets/datahub-ca.crt -days 365 -subj '/CN=ca.datahub/OU=test/O=datahub/L=paris/C=fr' -passin pass:datahub -passout pass:datahub >/dev/null 2>&1

echo " OK!"

for i in 'kafka' 'zookeeper' 'schema-registry' 'producer'
do
	printf "Creating cert and keystore of $i..."
	# Create keystores
	keytool -genkey -noprompt \
				 -alias $i \
				 -dname "CN=$i, OU=test, O=datahub, L=paris, C=fr" \
				 -keystore secrets/$i.keystore.jks \
				 -keyalg RSA \
				 -storepass datahub \
				 -keypass datahub  >/dev/null 2>&1

	# Create CSR, sign the key and import back into keystore
	keytool -keystore secrets/$i.keystore.jks -alias $i -certreq -file secrets/$i.csr -storepass datahub -keypass datahub >/dev/null 2>&1

	openssl x509 -req -CA secrets/datahub-ca.crt -CAkey secrets/datahub-ca.key -in secrets/$i.csr -out secrets/$i-ca-signed.crt -days 365 -CAcreateserial -passin pass:datahub  >/dev/null 2>&1

	keytool -keystore secrets/$i.keystore.jks -alias CARoot -import -noprompt -file secrets/datahub-ca.crt -storepass datahub -keypass datahub >/dev/null 2>&1

	keytool -keystore secrets/$i.keystore.jks -alias $i -import -file secrets/$i-ca-signed.crt -storepass datahub -keypass datahub >/dev/null 2>&1

	# Create truststore and import the CA cert.
	keytool -keystore secrets/$i.truststore.jks -alias CARoot -import -noprompt -file secrets/datahub-ca.crt -storepass datahub -keypass datahub >/dev/null 2>&1
	

  echo " OK!"
done

echo "datahub" > secrets/cert_creds
rm -rf tmp

echo "SUCCEEDED"

openssl genrsa -des3 -passout pass:datahub -out ./secrets/client.key 2048  >/dev/null 2>&1
openssl req -new -key ./secrets/client.key -out ./secrets/client.csr -subj "/CN=ca.datahub" -passin pass:datahub  >/dev/null 2>&1
openssl x509 -req -CA ./secrets/datahub-ca.crt -CAkey ./secrets/datahub-ca.key -in ./secrets/client.csr -out ./secrets/client.crt -days 365 -CAcreateserial -passin pass:datahub  >/dev/null 2>&1

# Verify the certificate
openssl verify -CAfile ./secrets/datahub-ca.crt ./secrets/client.crt  >/dev/null 2>&1

echo "Generate client keystore"
