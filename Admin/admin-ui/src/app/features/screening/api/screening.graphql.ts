import { gql } from 'apollo-angular';
import {
  CreateScreeningRequestInput,
  ScreeningFilters,
  ScreeningSort,
  UpdateScreeningRequestInput,
} from './screening.graphql.types';

const escapeGraphqlString = (value: string): string => value.replace(/\\/g, '\\\\').replace(/"/g, '\\"');

const buildInOrEqClause = (field: string, value?: string | string[]): string | null => {
  if (Array.isArray(value)) {
    if (value.length === 0) {
      return null;
    }

    const values = value.map((item) => `"${escapeGraphqlString(item)}"`).join(', ');
    return `${field}: { in: [${values}] }`;
  }

  if (!value) {
    return null;
  }

  return `${field}: { eq: "${escapeGraphqlString(value)}" }`;
};

const buildScreeningsWhereClause = (filters: ScreeningFilters): string => {
  const conditions: string[] = [];

  const movieCondition = buildInOrEqClause('movieId', filters.movieId);
  const hallCondition = buildInOrEqClause('hallId', filters.hallId);

  if (movieCondition) {
    conditions.push(movieCondition);
  }

  if (hallCondition) {
    conditions.push(hallCondition);
  }

  if (filters.date?.trim()) {
    conditions.push(`date: { eq: "${escapeGraphqlString(filters.date.trim())}" }`);
  }

  if (conditions.length === 0) {
    return '';
  }

  return `where: { ${conditions.join(', ')} }`;
};

export const getScreeningsPageQuery = (
  skip: number,
  take: number,
  sort: ScreeningSort,
  filters: ScreeningFilters,
) => ({
  query: gql`
    query getScreeningsPage($skip: Int!, $take: Int!) {
      screenings(
        skip: $skip,
        take: $take,
        order: { ${sort.field}: ${sort.direction} }
        ${buildScreeningsWhereClause(filters)}
      ) {
        items {
          id
          movieId
          hallId
          date
          startTime
          endTime
          ticketPrice
          dateCreated
        }
        totalCount
      }
    }
  `,
  variables: { skip, take },
  context: {
    allowAnonymous: true,
  },
});

export const getScreeningByIdQuery = (id: string) => ({
  query: gql`
    query getScreeningById($id: String!) {
      screeningById(id: $id) {
        id
        movieId
        hallId
        date
        startTime
        endTime
        ticketPrice
        dateCreated
      }
    }
  `,
  variables: { id },
  context: {
    allowAnonymous: true,
  },
});

export const createScreeningMutation = (request: CreateScreeningRequestInput) => ({
  mutation: gql`
    mutation createScreeningMutation($request: CreateScreeningRequestInput!) {
      createScreening(request: $request)
    }
  `,
  variables: { request },
});

export const updateScreeningMutation = (request: UpdateScreeningRequestInput) => ({
  mutation: gql`
    mutation updateScreeningMutation($request: UpdateScreeningRequestInput!) {
      updateScreening(request: $request)
    }
  `,
  variables: { request },
});

export const deleteScreeningMutation = (id: string) => ({
  mutation: gql`
    mutation deleteScreeningMutation($id: String!) {
      deleteScreening(id: $id)
    }
  `,
  variables: { id },
});
