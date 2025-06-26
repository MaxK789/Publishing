# Secret Management

Use [HashiCorp Vault](https://www.vaultproject.io/) to store sensitive configuration like database passwords and JWT signing keys.

1. Enable the KV secrets engine and write secrets under a path such as `publishing/`.
2. Configure Kubernetes authentication so pods can retrieve secrets at startup.
3. Update service manifests to use Vault agents or init containers that render secrets to environment variables.

Plain Kubernetes Secrets are still provided for local testing but should not be used in production.
