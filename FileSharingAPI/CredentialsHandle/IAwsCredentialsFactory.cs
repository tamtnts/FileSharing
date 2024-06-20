using Amazon.Runtime;

namespace FileSharingAPI.CredentialsHandle
{
    public interface IAwsCredentialsFactory
    {
        Task<AWSCredentials> GenerateAWSCredentialsAsync();
    }
}
