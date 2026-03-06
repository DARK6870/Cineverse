import { gql } from 'apollo-angular';
import { BookingFilters, BookingSort } from './booking.graphql.types';

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

const buildBookingsWhereClause = (filters: BookingFilters): string => {
  const conditions: string[] = [];

  const userCondition = buildInOrEqClause('userId', filters.userId);
  const screeningCondition = buildInOrEqClause('screeningId', filters.screeningId);

  if (userCondition) {
    conditions.push(userCondition);
  }

  if (screeningCondition) {
    conditions.push(screeningCondition);
  }

  if (conditions.length === 0) {
    return '';
  }

  return `where: { ${conditions.join(', ')} }`;
};

export const getBookingsPageQuery = (
  skip: number,
  take: number,
  sort: BookingSort,
  filters: BookingFilters,
) => ({
  query: gql`
    query getBookingsPage($skip: Int!, $take: Int!) {
      bookings(
        skip: $skip,
        take: $take,
        order: { ${sort.field}: ${sort.direction} }
        ${buildBookingsWhereClause(filters)}
      ) {
        items {
          id
          userId
          screeningId
          seatIds
          totalPrice
          dateCreated
        }
        totalCount
      }
    }
  `,
  variables: { skip, take }
});
