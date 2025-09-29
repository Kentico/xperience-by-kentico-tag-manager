# Jira Integration

This repository includes an automated GitHub Actions workflow that creates Jira tasks whenever new GitHub issues are opened.

## Overview

The Jira integration workflow (`jira-integration.yml`) automatically:
- Triggers when a new issue is created in the GitHub repository
- Extracts issue information (title, description, author, labels)
- Maps GitHub issue types to appropriate Jira issue types
- Creates a corresponding task in Jira Cloud
- Adds a comment back to the GitHub issue confirming the Jira task creation

## Setup Requirements

### 1. Jira Cloud Configuration

You need to have:
- A Jira Cloud instance
- A Jira project where tasks will be created
- API access credentials (email and API token)

### 2. GitHub Repository Secrets

The workflow requires the following secrets to be configured in your GitHub repository settings:

| Secret Name | Description | Example |
|-------------|-------------|---------|
| `JIRA_BASE_URL` | Your Jira Cloud base URL | `https://yourcompany.atlassian.net` |
| `JIRA_USER_EMAIL` | Email address of the Jira user | `user@company.com` |
| `JIRA_API_TOKEN` | Jira API token for authentication | `ATATT3xFfGF0...` |
| `JIRA_PROJECT_KEY` | Key of the Jira project | `PROJ` |

### 3. Setting Up GitHub Secrets

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret** for each required secret
4. Add the name and value for each secret listed above

### 4. Creating a Jira API Token

1. Log in to your Jira Cloud instance
2. Go to [Account Settings](https://id.atlassian.com/manage-profile/security/api-tokens)
3. Click **Create API token**
4. Give it a descriptive name (e.g., "GitHub Integration")
5. Copy the generated token and use it as `JIRA_API_TOKEN`

## How It Works

### Issue Type Mapping

The workflow automatically maps GitHub issue labels to Jira issue types:

| GitHub Labels | Jira Issue Type |
|---------------|-----------------|
| `bug`, `Bug` | Bug |
| `feature`, `Feature`, `enhancement` | Story |
| `task`, `Task` | Task |
| *Default* | Task |

### Jira Task Content

The created Jira task includes:

**Summary:** The GitHub issue title

**Description:** Contains:
- GitHub issue author information
- Repository name and link to the repository
- Direct link to the GitHub issue
- GitHub issue labels
- Original GitHub issue description

### Workflow Triggers

The workflow triggers on:
- New GitHub issues being opened (`issues.opened` event)

## Example

When a GitHub issue titled "Fix memory leak in tag rendering" with label "bug" is created by user "developer123", the workflow will:

1. Create a Jira Bug with the title "Fix memory leak in tag rendering"
2. Include the issue description with metadata about the author, repository, and link
3. Add a comment to the GitHub issue confirming the Jira task creation

## Troubleshooting

### Common Issues

1. **Workflow doesn't trigger:**
   - Ensure the workflow file is in the `main` branch
   - Check that all required secrets are configured

2. **Jira task creation fails:**
   - Verify Jira credentials and API token
   - Ensure the Jira project key exists and the user has permission to create issues
   - Check that the issue type exists in the Jira project

3. **Authentication errors:**
   - Verify the `JIRA_USER_EMAIL` matches the account that created the API token
   - Ensure the API token is valid and not expired

### Viewing Workflow Logs

1. Go to the **Actions** tab in your GitHub repository
2. Click on the "Jira Integration: Create Task from Issue" workflow
3. Select a specific run to view detailed logs

## Customization

The workflow can be customized by:

1. **Modifying issue type mapping:** Edit the label conditions in the `extract-type` step
2. **Changing Jira field mapping:** Modify the `description` field in the `Create Jira Task` step
3. **Adding additional Jira fields:** Extend the `atlassian/gajira-create` action with more fields
4. **Customizing the GitHub comment:** Edit the comment template in the final step

## Security Considerations

- API tokens should be treated as passwords and never exposed in code
- Use GitHub repository secrets for all sensitive configuration
- Regularly rotate API tokens
- Ensure Jira user has minimal required permissions

## Dependencies

The workflow uses these GitHub Actions:
- `atlassian/gajira-create@v3` - Creates Jira issues
- `actions/github-script@v7` - Adds comments to GitHub issues

These actions are maintained by their respective organizations and automatically updated.