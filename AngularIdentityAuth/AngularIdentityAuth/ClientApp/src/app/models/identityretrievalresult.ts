import { Claims } from "./claims";

export interface IdentityRetrievalResult {
  success: boolean,
  claims: Claims | undefined
}