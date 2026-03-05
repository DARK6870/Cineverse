import { gql } from 'apollo-angular';
import { UpdateUserRequestInput, UserDistinctField, UserFilters, UserSort } from './user.graphql.types';

const escapeGraphqlString = (value: string): string => value.replace(/\\/g, '\\\\').replace(/"/g, '\\"');

const buildUsersWhereClause = (filters: UserFilters): string => {
  const conditions: string[] = [];

  if (filters.status?.length) {
    const values = filters.status.map((status) => escapeGraphqlString(status)).join(', ');
    conditions.push(`status: { in: [${values}] }`);
  }

  if (filters.role?.length) {
    const values = filters.role.map((role) => escapeGraphqlString(role)).join(', ');
    conditions.push(`role: { in: [${values}] }`);
  }

  if (filters.provider?.length) {
    const values = filters.provider.map((provider) => escapeGraphqlString(provider)).join(', ');
    conditions.push(`provider: { in: [${values}] }`);
  }

  if (!conditions.length) {
    return '';
  }

  return `where: { ${conditions.join(', ')} }`;
};

export const getUserByIdQuery = (id: string) => ({
  query: gql`
    query getUserById($id: String!) {
      userById(id: $id) {
        id
        email
        firstName
        lastName
        provider
        role
        status
      }
    }
  `,
  variables: { id },
});

export const getUsersPageQuery = (
  skip: number,
  take: number,
  sort: UserSort,
  filters: UserFilters,
  searchTerm?: string,
) => ({
  query: gql`
    query getUsersPage($skip: Int!, $take: Int!, $searchTerm: String) {
      users(
        skip: $skip,
        take: $take,
        searchTerm: $searchTerm,
        order: { ${sort.field}: ${sort.direction} }
        ${buildUsersWhereClause(filters)}
      ) {
        items {
          id
          email
          firstName
          lastName
          provider
          role
          status
        }
        totalCount
      }
    }
  `,
  variables: { skip, take, searchTerm },
});

export const getUsersDistinctFilterValuesQuery = (field: UserDistinctField) => ({
  query: gql`
    query getUsersDistinctFilterValues {
      usersDistinctFilterValues(field: ${field})
    }
  `,
});

export const updateUserMutation = (request: UpdateUserRequestInput) => ({
  mutation: gql`
    mutation updateUser($request: UpdateUserRequestInput!) {
      updateUser(request: $request)
    }
  `,
  variables: { request },
});
