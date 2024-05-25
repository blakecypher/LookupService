# Lookup Service - Level 1
Below, you'll find the instructions for getting started with your task. Please read them carefully to avoid unexpected issues. Best of luck!

## Time estimate

Between 2 and 3 hours, plus the time to set up the codebase.

## The task

<!--TASK_INSTRUCTIONS_START-->

Your task is to build a backend service that implements the [Lookup Service REST API](https://infra.devskills.app/lookup/api/1.0.0) and integrates with the [Credit Data REST API](https://infra.devskills.app/credit-data/api/1.0.0) to aggregate historical credit data.

<details>
<summary>Lookup Service REST API Specification</summary>

```json
{
  "openapi": "3.0.0",
  "info": {
    "title": "Lookup Service API",
    "version": "1.0.0"
  },
  "paths": {
    "/ping": {
      "get": {
        "summary": "Healhcheck to make sure the service is up.",
        "responses": {
          "200": {
            "description": "The service is up and running."
          }
        }
      }
    },
    "/credit-data/{ssn}": {
      "get": {
        "summary": "Return aggregated credit data.",
        "parameters": [
          {
            "name": "ssn",
            "in": "path",
            "required": true,
            "description": "Social security number.",
            "schema": {
              "type": "string"
            },
            "example": "424-11-9327"
          }
        ],
        "responses": {
          "200": {
            "description": "Aggregated credit data for given ssn.",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/CreditData"
                },
                "examples": {
                  "CreditDataEmma": {
                    "$ref": "#/components/examples/CreditDataEmma"
                  }
                }
              }
            }
          },
          "404": {
            "description": "Credit data not found for given ssn."
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "CreditData": {
        "type": "object",
        "properties": {
          "first_name": {
            "type": "string"
          },
          "last_name": {
            "type": "string"
          },
          "address": {
            "type": "string"
          },
          "assessed_income": {
            "type": "integer"
          },
          "balance_of_debt": {
            "type": "integer"
          },
          "complaints": {
            "type": "boolean"
          }
        }
      }
    },
    "examples": {
      "CreditDataEmma": {
        "value": {
          "first_name": "Emma",
          "last_name": "Gautrey",
          "address": "09 Westend Terrace",
          "assessed_income": 60668,
          "balance_of_debt": 11585,
          "complaints": true
        }
      }
    }
  }
}
```
</details>

<details>
<summary>Credit Data REST API Specification</summary>

```json
{
  "openapi": "3.0.0",
  "info": {
    "title": "Credit Data API",
    "version": "1.0.0"
  },
  "paths": {
    "/personal-details/{ssn}": {
      "get": {
        "summary": "Return personal details.",
        "parameters": [
          {
            "name": "ssn",
            "in": "path",
            "required": true,
            "description": "Social security number.",
            "schema": {
              "type": "string"
            },
            "example": "424-11-9327"
          }
        ],
        "responses": {
          "200": {
            "description": "Personal details for given ssn.",
            "headers": {
              "Cache-Control": {
                "schema": {
                  "type": "string"
                },
                "description": "Cache-Control response directives, e.g. \"private, max-age=604800\" or \"no-store\""
              }
            },
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/PersonalDetails"
                },
                "examples": {
                  "PersonalDetailsEmma": {
                    "$ref": "#/components/examples/PersonalDetailsEmma"
                  }
                }
              }
            }
          },
          "404": {
            "description": "Personal data not found for given ssn."
          }
        }
      }
    },
    "/assessed-income/{ssn}": {
      "get": {
        "summary": "Return assessed details income.",
        "parameters": [
          {
            "name": "ssn",
            "in": "path",
            "required": true,
            "description": "Social security number.",
            "schema": {
              "type": "string"
            },
            "example": "424-11-9327"
          }
        ],
        "responses": {
          "200": {
            "description": "Assessed income details for given ssn.",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/AssessedIncomeDetails"
                },
                "examples": {
                  "AssessedIncomeEmma": {
                    "$ref": "#/components/examples/AssessedIncomeDetailsEmma"
                  }
                }
              }
            }
          },
          "404": {
            "description": "Personal data not found for given ssn."
          }
        }
      }
    },
    "/debt/{ssn}": {
      "get": {
        "summary": "Return debt details.",
        "parameters": [
          {
            "name": "ssn",
            "in": "path",
            "required": true,
            "description": "Social security number.",
            "schema": {
              "type": "string"
            },
            "example": "424-11-9327"
          }
        ],
        "responses": {
          "200": {
            "description": "Debt details for given ssn.",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/DebtDetails"
                },
                "examples": {
                  "DebtEmma": {
                    "$ref": "#/components/examples/DebtDetailsEmma"
                  }
                }
              }
            }
          },
          "404": {
            "description": "Personal data not found for given ssn."
          }
        }
      }
    }
  },
  "servers": [
    {
      "url": "https://infra.devskills.app/api/credit-data"
    }
  ],
  "components": {
    "schemas": {
      "PersonalDetails": {
        "type": "object",
        "properties": {
          "first_name": {
            "type": "string"
          },
          "last_name": {
            "type": "string"
          },
          "address": {
            "type": "string"
          }
        }
      },
      "AssessedIncomeDetails": {
        "type": "object",
        "properties": {
          "assessed_income": {
            "type": "integer"
          }
        }
      },
      "DebtDetails": {
        "type": "object",
        "properties": {
          "balance_of_debt": {
            "type": "integer"
          },
          "complaints": {
            "type": "boolean"
          }
        }
      }
    },
    "examples": {
      "PersonalDetailsEmma": {
        "value": {
          "first_name": "Emma",
          "last_name": "Gautrey",
          "address": "09 Westend Terrace"
        }
      },
      "AssessedIncomeDetailsEmma": {
        "value": {
          "assessed_income": 60668
        }
      },
      "DebtDetailsEmma": {
        "value": {
          "balance_of_debt": 11585,
          "complaints": true
        }
      }
    }
  }
}
```
</details>
