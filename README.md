# F1 Data API

## Overview
This API provides Foormula 1 historical racing data through a RESTFUL interface.
Available data is from 1980. 

**Note on Data Completeness:**
- Data coverage: 1980 - present
- Some records may be incomplete or missing

[View Interactive API Documentation](https://f1dataapi.depanshu.com/scalar/v1)

## Tech Stack
- **.NET**
- **C#** 
- **MongoDB** - For data storage
### Availble at
## Quick Start
Base URL: `https://f1dataapi.depanshu.com/`

Example requests:
```http
GET /api/drivers?id=1          # Get specific driver
GET /api/races?season=2023     # Get all races from 2023
GET /api/constructors          # List all constructors
```

## API Documentation
Interactive documentation with full endpoint details is available throug [Scalar API Reference](https://f1dataapi.depanshu.com/scalar/v1).


## Data Source
This API's historical Formula 1 data is sourced from Randy Connolly's F1 API- https://www.randyconnolly.com/funwebdev/3rd/api/f1/


