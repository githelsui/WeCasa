import app from '../ClientApp/src/App'; // replace with the path to your app.js file
import request from 'supertest';

describe('GET group-settings/${group.groupId}', () => {
    it('responds with JSON containing a list of all users', (done) => {
      request(app)
        .get('/api/users')
        .set('Accept', 'application/json')
        .expect('Content-Type', /json/)
        .expect(200)
        .end((err, res) => {
          if (err) return done(err);
          expect(res.body.length).toBeGreaterThan(0);
          done();
        });
    });
  });

  

