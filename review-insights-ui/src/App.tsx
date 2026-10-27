
import { useEffect, useState, type FormEvent } from 'react'
import './App.css'

type Sentiment = 'Positive' | 'Neutral' | 'Negative'

type Review = {
  id: string
  reviewText: string
  sentiment: Sentiment
  category: string
  summary: string
  createdAt: string
}

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

function App() {
  const [reviewText, setReviewText] = useState('')
  const [reviews, setReviews] = useState<Review[]>([])
  const [filter, setFilter] = useState('All')
  const [loading, setLoading] = useState(true)
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    async function fetchReviews() {
      try {
        setLoading(true)
        setError('')

        const response = await fetch(`${API_BASE_URL}/api/reviews`)

        if (!response.ok) {
          throw new Error('Failed to load reviews.')
        }

        const data: Review[] = await response.json()
        setReviews(data)
      } catch (err) {
        setError(
          err instanceof Error ? err.message : 'Something went wrong.',
        )
      } finally {
        setLoading(false)
      }
    }

    fetchReviews()
  }, [])

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const text = reviewText.trim()

    if (!text || submitting) return

    try {
      setSubmitting(true)
      setError('')

      const response = await fetch(`${API_BASE_URL}/api/reviews`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ reviewText: text }),
      })

      if (!response.ok) {
        throw new Error('Failed to analyze the review. Please try again.')
      }

      const newReview: Review = await response.json()

      setReviews((current) => [newReview, ...current])
      setReviewText('')
    } catch (err) {
      setError(
        err instanceof Error ? err.message : 'Something went wrong.',
      )
    } finally {
      setSubmitting(false)
    }
  }

  const filteredReviews =
    filter === 'All'
      ? reviews
      : reviews.filter((review) => review.sentiment === filter)

  const positiveCount = reviews.filter(
    (review) => review.sentiment === 'Positive',
  ).length

  const negativeCount = reviews.filter(
    (review) => review.sentiment === 'Negative',
  ).length

  function sentimentClass(sentiment: Sentiment) {
    return `badge badge-${sentiment.toLowerCase()}`
  }

  return (
    <main className="dashboard">
      <header className="topbar">
        <a className="brand" href="/">
          <span className="brand-mark">R</span>
          <span>
            Review<span className="brand-accent">Insights</span>
          </span>
        </a>

        <div className="topbar-right">
          <span className="status-dot" />
          <span>Review analytics</span>
          <div className="avatar">RI</div>
        </div>
      </header>

      <section className="page-content">
        <div className="page-heading">
          <div>
            <p className="eyebrow">CUSTOMER EXPERIENCE</p>
            <h1>Review intelligence</h1>
            <p className="subtitle">
              Understand what your customers are saying, at a glance.
            </p>
          </div>
          <div className="demo-label">DEMO DATA</div>
        </div>

        <section className="stats-grid">
          <article className="stat-card">
            <span className="stat-label">Total reviews</span>
            <strong className="stat-number">{reviews.length}</strong>
            <span className="stat-note">Reviews in this session</span>
          </article>

          <article className="stat-card">
            <span className="stat-label">Positive reviews</span>
            <strong className="stat-number positive-text">
              {positiveCount}
            </strong>
            <span className="stat-note">Positive customer feedback</span>
          </article>

          <article className="stat-card">
            <span className="stat-label">Negative reviews</span>
            <strong className="stat-number negative-text">
              {negativeCount}
            </strong>
            <span className="stat-note">Potential issues to investigate</span>
          </article>
        </section>

        <section className="panel composer-panel">
          <div className="panel-heading">
            <div>
              <h2>Analyze a review</h2>
              <p>Enter customer feedback to get started.</p>
            </div>
            <span className="ai-chip">
              <span>✦</span> AI-powered
            </span>
          </div>

          <form onSubmit={handleSubmit}>
            <label htmlFor="reviewText">Customer review</label>
            <textarea
              id="reviewText"
              value={reviewText}
              onChange={(event) => setReviewText(event.target.value)}
              placeholder="e.g. I love the new dashboard, but the app is slow when loading reports..."
              maxLength={5000}
              rows={4}
            />

            <div className="form-footer">
              <span>{reviewText.length}/5000 characters</span>
              <button type="submit" disabled={!reviewText.trim() || submitting}>
                <span>✦</span> {submitting ? 'Analyzing...' : 'Add review'}
              </button>
            </div>
          </form>
          <p className="demo-note">
            Demo mode: live AI analysis will be connected in the next step.
          </p>
        </section>

        <section className="panel reviews-panel">
          <div className="panel-heading reviews-heading">
            <div>
              <h2>Review history</h2>
              <p>Browse and explore customer feedback.</p>
            </div>

            <select
              aria-label="Filter by sentiment"
              value={filter}
              onChange={(event) => setFilter(event.target.value)}
            >
              <option value="All">All sentiments</option>
              <option value="Positive">Positive</option>
              <option value="Neutral">Neutral</option>
              <option value="Negative">Negative</option>
            </select>
          </div>

          {loading && <p>Loading reviews...</p>}
          {error && <p role="alert">{error}</p>}
          <div className="table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>Customer review</th>
                  <th>Sentiment</th>
                  <th>Category</th>
                  <th>AI summary</th>
                  <th>Date</th>
                </tr>
              </thead>
              <tbody>
                {filteredReviews.map((review) => (
                  <tr key={review.id}>
                    <td className="review-cell">{review.reviewText}</td>
                    <td>
                      <span className={sentimentClass(review.sentiment)}>
                        <span className="badge-dot" />
                        {review.sentiment}
                      </span>
                    </td>
                    <td>
                      <span className="category-chip">{review.category}</span>
                    </td>
                    <td className="summary-cell">{review.summary}</td>
                    <td className="date-cell">
                      {new Date(review.createdAt).toLocaleDateString()}
                    </td>
                  </tr>
                ))}
                {filteredReviews.length === 0 && (
                  <tr>
                    <td colSpan={5} className="empty-state">
                      No reviews match this filter.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>

          <div className="table-footer">
            Showing {filteredReviews.length} of {reviews.length} reviews
          </div>
        </section>

        <footer className="page-footer">
          <span>ReviewInsights</span>
          <span>Customer feedback, made actionable.</span>
        </footer>
      </section>
    </main>
  )
}

export default App
